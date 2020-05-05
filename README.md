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

# Samples
    class Samples
    {
        public static int A(int x, int y)
        {
            Console.WriteLine($"Program.A({x}, {y})");
            return x + y;
        }

        public static int B(int x, int y)
        {
            Console.WriteLine($"Program.B({x}, {y})");
            return x + y;
        }

        public class Foo
        {
            public void A()
            {
                Console.WriteLine("Foo.A");
            }

            public void B()
            {
                Console.WriteLine("Foo.B");
            }

            public void G(int x)
            {
                Console.WriteLine($"Foo.G{x}");
            }
        };

        public static void G(Foo foo, int x)
        {
            Console.WriteLine($"Program.Foo.G({x})");
        }

        static void Main(string[] args)
        {
            Interceptor[] interceptor = new[]
            {
                new Interceptor(typeof(Samples).GetMethod("A"), typeof(Samples).GetMethod("B")),
                new Interceptor(typeof(Foo).GetMethod("A"), typeof(Foo).GetMethod("B")),
                new Interceptor(typeof(Foo).GetMethod("G"), typeof(Samples).GetMethod("G")),
            };

            A(1, 2);

            Foo foo = new Foo();
            foo.A();

            foo.G(3);
            interceptor[2].Invoke(() => foo.G(3));

            Console.ReadKey(false);
        }
    }
