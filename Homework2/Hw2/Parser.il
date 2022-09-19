.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{

    .method public static void ParseCalcArguments(string[] args,
        [out] float64& val1,
        [out] valuetype Hw2.CalculatorOperation& operation,
        [out] float64& val2)
    {
        ldarg.0         
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        brfalse.s EXCEPTION
        
        ldarg.0
        ldc.i4.0
        ldelem.ref      
        ldarg val1    
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse.s EXCEPTION

        ldarg.0
        ldc.i4.2
        ldelem.ref      
        ldarg val2      
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse.s EXCEPTION

        ldarg operation
        ldarg.0
        ldc.i4.1
        ldelem.ref      
        call valuetype Hw2.CalculatorOperation [Hw2]Hw2.Parser::ParseOperation(string)
        stind.i4         

        ret

        EXCEPTION:
            newobj instance void [System.Runtime]System.ArgumentException::.ctor()
            throw       
    }

    .method private static bool IsArgLengthSupported(string[] args) cil managed
    {
        ldarg args
        ldlen
        ldc.i4.3
        ceq
        ret
    }

    .method private static valuetype Hw2.CalculatorOperation ParseOperation(string arg) cil managed
    {
        ldarg arg
        ldstr       "+"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s    PLUS 
        
        ldarg arg
        ldstr       "-"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s    MINUS
        
        ldarg arg
        ldstr       "*"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s    MULTIPLY
        
        ldarg arg
        ldstr       "/"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s    DIVIDE 
        
        newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
        throw
        
        PLUS:
            ldc.i4.0
            ret

        MINUS: 
            ldc.i4.1
            ret

        MULTIPLY:
            ldc.i4.2
            ret

        DIVIDE:
            ldc.i4.3
            ret
    }
}


