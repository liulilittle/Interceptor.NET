namespace EChat
{
    using System;
    using EChat.Hooking;

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
}
