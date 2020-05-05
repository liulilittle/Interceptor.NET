# Interceptor.NET
.net clr /core (x86/x64 instruction set platform) managed function inline interceptor (hooking)

# Restrictions
1、OS: <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1.1 >= Linux(:centos-7)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1.1 >= Windows-XP<br/>
2、CPU: Must support IA32/IA64 instruction-set
<br/>
3、RT：<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3.1 >= .NET Core 2.0<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3.2 >= .NET Framework 2.0<br/>

# Usage
1、Install
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;var interceptor = new Interceptor(typeof(Samples).GetMethod("A"), typeof(Samples).GetMethod("B"));
<br/>
2、Suspend
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;interceptor.Suspend();
<br/>
2、Resume
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;interceptor.Resume();
<br/>
