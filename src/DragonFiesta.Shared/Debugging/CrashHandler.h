#ifndef CRASHANDLER_H
#define CRASHANDLER_H


#include <execinfo.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ucontext.h>
#include <unistd.h>


class CrashHandler
{
public :


    static void crit_err_hdlr(int sig_num, siginfo_t * info, void * ucontext);
    static void Start();

};

typedef struct _sig_ucontext
{
    unsigned long     uc_flags;
    struct ucontext   *uc_link;
    stack_t           uc_stack;
    struct sigcontext uc_mcontext;
    sigset_t          uc_sigmask;
} sig_ucontext_t;

#endif
