namespace Console.Test
{
    using No.Added.Parser;
    using No.Added.Parser.Code;
    using No.Added.Parser.Expressions;
    using System;
    using System.Collections.Generic;

    public class Visitor : IVisitor
    {
        public void Visit(AdditiveExpression node)
        {
            Console.Write("AdditiveExpression");
            Console.Write(" Left => " + node.Left.GetType().Name);
            Console.WriteLine(" Right => " + node.Right.GetType().Name);
        }

        public void Visit(EqualityExpression node)
        {
            Console.Write("EqualityExpression");
            Console.Write(" Left => " + node.Left.GetType().Name);
            Console.WriteLine(" Right => " + node.Right.GetType().Name);
        }

        public void Visit(LogicalExpression node)
        {
            Console.Write("LogicalExpression");
            Console.Write(" Left => " + node.Left.GetType().Name);
            Console.WriteLine(" Right => " + node.Right.GetType().Name);
        }

        public void Visit(GroupExpression node)
        {
            Console.Write("GroupExpression");
            Console.WriteLine(" Node => " + node.Node.GetType().Name);
        }

        public void Visit(UnaryExpression node)
        {
            Console.Write("UnaryExpression");
            Console.WriteLine(" Node => " + node.Node.GetType().Name);
        }

        public void Visit(ArgumentList node)
        {
            Console.WriteLine("ArgumentList");
        }

        public void Visit(MemberExpression node)
        {
            Console.Write("MemberExpression");
            Console.Write(" Left => " + node.Left.GetType().Name);
            Console.WriteLine(" Right => " + node.Right.GetType().Name);
        }

        public void Visit(CallExpression node)
        {
            Console.Write("CallExpression ");
        }


        public void Visit(Identifier node)
        {
            Console.WriteLine("Identifier => " + node.Value);
        }

        public void Visit(ElementList node)
        {

        }

        public void Visit(DecimalLiteral node)
        {
            Console.WriteLine("DecimalLiteral => " + node.Value);
        }



        public void Visit(IntegerLiteral node)
        {
            Console.WriteLine("IntegerLiteral => " + node.Value);
        }


        public void Visit(RelationalExpression node)
        {

        }

        public void Visit(MultiplicativeExpression node)
        {

        }



        public void Visit(StringLiteral node)
        {
            Console.WriteLine("StringLiteral => " + node.Value);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var visitor = new Visitor();
            var testing = new List<string>();
            testing.Add(@"('Aap Noot') == ('Boefjes\'\nscheet') && ((5) == (7))");
            testing.Add(@"('Aap Noot') == ('Boefjes\'\nscheet') && ((5)==( 7))");

            testing.Add(@"-3== 0xF5 +.790+0e-098-0E-0");
            testing.Add(@"-(3)== 0xF5 +.790+0e-098-0E-0");

            testing.Add(@"aap( a == b && 3 != 4)");
            testing.Add(@"poes(.34, -7, 'TESTING' == 'SNOT')");
            testing.Add(@"('Aap Noot')==('Boefjes\'\nscheet')&&((5)==(678))");

            testing.Add(@" (('Aap Noot' == 'Boefjes\'\nscheet' && 5 == 7 ) || ( aap != poes) ) && (-3== 0xF5 +.790+0e-098-0E-0)");
            testing.Add(@" (('Aap Noot' == 'Boefjes\'\nscheet' && 5 == 7 ) || (! poep.snot.konijn.aap( a == b && 3 != 4) != poes (.34, -7, 'TESTING' == 'SNOT')) ) && (-3== 0xF5 +.790+1e-5-0E-0)");
            testing.Add(@"poep.snot.konijn.aap");
            do
            {
                var wait = Console.ReadLine();
                foreach (var test in testing)
                {
                    var parser = new DefaultParser();
                    var node = parser.Parse(new RawScript(test));
                    if (node != null)
                    {
                        node.Accept(visitor);
                    }

                    Console.WriteLine("--------------------------------");
                }
                Console.WriteLine("END");

            } while (Console.ReadLine() != "S");
        }
    }
}
