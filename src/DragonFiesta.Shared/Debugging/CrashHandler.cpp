#ifndef _GNU_SOURCE
#define _GNU_SOURCE
#endif
#ifndef __USE_GNU
#define __USE_GNU
#endif
#include "CrashHandler.h"
#include "Database/DatabaseManager.hpp"
#include <execinfo.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ucontext.h>
#include <unistd.h>
#include <iostream>
#include <fstream>
#include "boost/filesystem.hpp"
#include "Util/DateTime.h"
#include "Log/Log.h"
#include "Util/StringExtentsion.h"
/* This structure mirrors the one found in /usr/include/asm/ucontext.h */

using namespace std;

std::ofstream outsStream;

void CrashHandler::crit_err_hdlr(int sig_num, siginfo_t * info, void * ucontext)
{
    void *             array[50];
    void *             caller_address;
    char **            messages;
    int                size, i;
    sig_ucontext_t *   uc;

    uc = (sig_ucontext_t *)ucontext;

    /* Get the address at the time the signal was raised */
#if defined(__i386__) // gcc specific
    caller_address = (void *) uc->uc_mcontext.eip; // EIP: x86 specific
#elif defined(__x86_64__) // gcc specific
    caller_address = (void *) uc->uc_mcontext.rip; // RIP: x86_64 specific
#else
#error Unsupported architecture. // TODO: Add support for other arch.
#endif

    string sigstring = MakeString(stderr, " signal ",sig_num, strsignal(sig_num)," address is ",info->si_addr," from ",(void *)caller_address);
    outsStream << sigstring << endl;
    sLog->LogMSG(MSG_ERROR,sigstring.c_str());
    size = backtrace(array, 50);

    /* overwrite sigaction with caller's address */
    array[1] = caller_address;
    string sigstring2 = MakeString(stderr, " signal ",sig_num, strsignal(sig_num)," address is ",info->si_addr," from ",(void *)caller_address);
    std::cout << sigstring2 << endl;
    messages = backtrace_symbols(array, size);

    /* skip first stack frame (points here) */
    for (i = 1; i < size && messages != NULL; ++i)
    {
        string Err = MakeString("[bt]: (",i,") ",messages[i]);

        sLog->LogMSG(MSG_ERROR,Err.c_str());
        outsStream << Err << std::endl;
    }

    free(messages);

    exit(EXIT_FAILURE);
}

void CrashHandler::Start()
{

    struct sigaction sigact;

    sigact.sa_sigaction = CrashHandler::crit_err_hdlr;
    sigact.sa_flags = SA_RESTART | SA_SIGINFO;


    if (!boost::filesystem::exists("Crashes"))
    {
        boost::filesystem::create_directory("Crashes");
    }
    outsStream.open(MakeString("Crashes/Crash_",DateTime::NowDate(),".txt"));

    if (sigaction(SIGSEGV, &sigact, (struct sigaction *)NULL) != 0)
    {
        std::string  Error = MakeString("error setting signal handler for ",SIGSEGV,strsignal(SIGSEGV),"\n"); //MakeString("error setting signal handler for ",SIGSEGV,strsignal(SIGSEGV),"\n");
        outsStream << Error << std::endl;

        exit(EXIT_FAILURE);
    }

}

/*CrashHandler::CrashHandler
{
std::cout << "tets";
}*/

/*
int main(int argc, char ** argv)
{
 struct sigaction sigact;

 sigact.sa_sigaction = crit_err_hdlr;
 sigact.sa_flags = SA_RESTART | SA_SIGINFO;

 if (sigaction(SIGSEGV, &sigact, (struct sigaction *)NULL) != 0)
 {
  fprintf(stderr, "error setting signal handler for %d (%s)\n",
    SIGSEGV, strsignal(SIGSEGV));

  exit(EXIT_FAILURE);
 }

 foo1();

 exit(EXIT_SUCCESS);
}*/
