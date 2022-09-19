.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
    .method static public int32 Calculate(int32 value1, valuetype [Hw2]Hw2.CalculatorOperation operation, int32 value2) cil managed
    {

        .entrypoint
        
        ldarg.0
        ldarg.2                 

        ldarg.1
        ldc.i4.0
        beq.s       PLUS     

        ldarg.1
        ldc.i4.1
        beq.s       MINUS     

        ldarg.1
        ldc.i4.2
        beq.s      MULTIPLY    

        ldarg.1
        ldc.i4.3
        beq.s       DIVIDE   

        br.s        EXCEPTION  

        PLUS:
            add
            ret

        MINUS:    
            sub
            ret

        MULTIPLY:   
            mul
            ret

        DIVIDE:    
            div
            ret

        DIVIDE_ZERO:
            newobj instance void [System.Runtime]System.DivideByZeroException::.ctor()
            throw
        
        EXCEPTION:    
            newobj instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
            throw
        
    }
}



