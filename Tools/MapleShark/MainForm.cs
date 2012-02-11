using SharpPcap;
using SharpPcap.Packets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MapleShark
{
    public partial class MainForm : Form
    {
        private bool mClosed = false;
        private PcapDevice mDevice = null;
        private SearchForm mSearchForm = new SearchForm();
        private DataForm mDataForm = new DataForm();
        private StructureForm mStructureForm = new StructureForm();
        private PropertyForm mPropertyForm = new PropertyForm();

        public MainForm(string[] pArgs)
        {
            InitializeComponent();
            Text = "MapleShark " + Program.AssemblyVersion;
            foreach (string arg in pArgs)
            {
                SessionForm session = NewSession();
                session.OpenReadOnly(arg);
            }
        }

        public SearchForm SearchForm { get { return mSearchForm; } }
        public DataForm DataForm { get { return mDataForm; } }
        public StructureForm StructureForm { get { return mStructureForm; } }
        public PropertyForm PropertyForm { get { return mPropertyForm; } }

        private SessionForm NewSession()
        {
            SessionForm session = new SessionForm();
            session.Show(mDockPanel, DockState.Document);
            return session;
        }

        public void CopyPacketHex(KeyEventArgs pArgs)
        {
            if (mDataForm.HexBox.SelectionLength > 0 && pArgs.Modifiers == Keys.Control && pArgs.KeyCode == Keys.C)
            {
                Clipboard.SetText(BitConverter.ToString((mDataForm.HexBox.ByteProvider as DynamicByteProvider).Bytes.ToArray(), (int)mDataForm.HexBox.SelectionStart, (int)mDataForm.HexBox.SelectionLength).Replace("-", " "));
                pArgs.SuppressKeyPress = true;
            }
            else if (mDataForm.HexBox.SelectionLength > 0 && pArgs.Control && pArgs.Shift && pArgs.KeyCode == Keys.C)
            {
                byte[] buffer = new byte[mDataForm.HexBox.SelectionLength];
                Buffer.BlockCopy((mDataForm.HexBox.ByteProvider as DynamicByteProvider).Bytes.ToArray(), (int)mDataForm.HexBox.SelectionStart, buffer, 0, (int)mDataForm.HexBox.SelectionLength);
                mSearchForm.HexBox.ByteProvider.DeleteBytes(0, mSearchForm.HexBox.ByteProvider.Length);
                mSearchForm.HexBox.ByteProvider.InsertBytes(0, buffer);
                pArgs.SuppressKeyPress = true;
            }
        }

        private void MainForm_Load(object pSender, EventArgs pArgs)
        {
            if (new SetupForm().ShowDialog(this) != DialogResult.OK) { Close(); return; }
            foreach (PcapDevice device in new PcapDeviceList())
            {
                if (device.Interface.FriendlyName == Config.Instance.Interface)
                {
                    mDevice = device;
                    break;
                }
            }
            mDevice.Open(true, 1);
            mDevice.SetFilter(string.Format("tcp portrange {0}-{1}", Config.Instance.LowPort, Config.Instance.HighPort));
            mTimer.Enabled = true;

            mSearchForm.Show(mDockPanel);
            mDataForm.Show(mDockPanel);
            mStructureForm.Show(mDockPanel);
            mPropertyForm.Show(mDockPanel);
            DockPane rightPane1 = new DockPane(mStructureForm, DockState.DockRight, true);
            DockPane rightPane2 = new DockPane(mPropertyForm, DockState.DockRight, true);
            rightPane1.Show();
            rightPane2.Show();
        }

        private void MainForm_FormClosed(object pSender, FormClosedEventArgs pArgs)
        {
            mTimer.Enabled = false;
            if (mDevice != null) mDevice.Close();
            mClosed = true;
        }

        private void mTimer_Tick(object pSender, EventArgs pArgs)
        {
            Packet packet = null;
            while ((packet = mDevice.GetNextPacket()) != null)
            {
                TCPPacket tcpPacket = packet as TCPPacket;
                SessionForm session = null;
                if (tcpPacket.Syn && !tcpPacket.Ack) session = NewSession();
                else session = Array.Find(MdiChildren, f => (f as SessionForm).MatchTCPPacket(tcpPacket)) as SessionForm;
                if (session != null) session.BufferTCPPacket(tcpPacket);
            }
        }

        private void mDockPanel_ActiveDocumentChanged(object pSender, EventArgs pArgs)
        {
            if (!mClosed)
            {
                SessionForm session = mDockPanel.ActiveDocument as SessionForm;
                mSearchForm.ComboBox.Items.Clear();
                if (session != null) session.RefreshPackets();
                else
                {
                    if (mDataForm.HexBox.ByteProvider != null) mDataForm.HexBox.ByteProvider.DeleteBytes(0, mDataForm.HexBox.ByteProvider.Length);
                    mStructureForm.Tree.Nodes.Clear();
                    mPropertyForm.Properties.SelectedObject = null;
                }
            }
        }

        private void mFileImportMenu_Click(object pSender, EventArgs pArgs)
        {
            if (mImportDialog.ShowDialog(this) != DialogResult.OK) return;
            PcapOfflineDevice device = new PcapOfflineDevice(mImportDialog.FileName);
            device.Open();

            Packet packet = null;
            SessionForm session = null;
            while ((packet = device.GetNextPacket()) != null)
            {
                TCPPacket tcpPacket = packet as TCPPacket;
                if (tcpPacket == null) continue;
                if ((tcpPacket.SourcePort < Config.Instance.LowPort || tcpPacket.SourcePort > Config.Instance.HighPort) &&
                    (tcpPacket.DestinationPort < Config.Instance.LowPort || tcpPacket.DestinationPort > Config.Instance.HighPort)) continue;
                if (tcpPacket.Syn && !tcpPacket.Ack) { session = NewSession(); session.BufferTCPPacket(tcpPacket); }
                else if (session.MatchTCPPacket(tcpPacket)) session.BufferTCPPacket(tcpPacket);
            }
            mSearchForm.RefreshOpcodes(false);
        }

        private void mFileOpenMenu_Click(object pSender, EventArgs pArgs)
        {
            if (mOpenDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string path in mOpenDialog.FileNames)
                {
                    SessionForm session = NewSession();
                    session.OpenReadOnly(mOpenDialog.FileName);
                }
                mSearchForm.RefreshOpcodes(false);
            }
        }

        private void mFileQuit_Click(object pSender, EventArgs pArgs)
        {
            Close();
        }

        private void mViewMenu_DropDownOpening(object pSender, EventArgs pArgs)
        {
            mViewSearchMenu.Checked = mSearchForm.Visible;
            mViewDataMenu.Checked = mDataForm.Visible;
            mViewStructureMenu.Checked = mStructureForm.Visible;
            mViewPropertiesMenu.Checked = mPropertyForm.Visible;
        }

        private void mViewSearchMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewSearchMenu.Checked) mSearchForm.Show();
            else mSearchForm.Hide();

        }

        private void mViewDataMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewDataMenu.Checked) mDataForm.Show();
            else mDataForm.Hide();
        }

        private void mViewStructureMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewStructureMenu.Checked) mStructureForm.Show();
            else mStructureForm.Hide();
        }

        private void mViewPropertiesMenu_CheckedChanged(object pSender, EventArgs pArgs)
        {
            if (mViewPropertiesMenu.Checked) mPropertyForm.Show();
            else mPropertyForm.Hide();
        }
    }
}
