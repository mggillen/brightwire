﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BrightWire.ExecutionGraph.Activation
{
    class Tanh : IComponent
    {
        class Backpropagation : IBackpropagation
        {
            readonly IMatrix _input;

            public Backpropagation(IMatrix matrix)
            {
                _input = matrix;
            }

            public void Dispose()
            {
                _input.Dispose();
            }

            public void Backward(IMatrix errorSignal, int channel, IBatchContext context, bool calculateOutput)
            {
                using (var od = _input.TanhDerivative())
                    context.Backpropagate(errorSignal.PointwiseMultiply(od), channel);
            }
        }

        public void Dispose()
        {
            // nop
        }

        public IMatrix Train(IMatrix input, int channel, IBatchContext context)
        {
            context.AddBackpropagation(new Backpropagation(input), channel);
            return Execute(input, context);
        }

        public IMatrix Execute(IMatrix input, IBatchContext context)
        {
            return input.TanhActivation();
        }
    }
}
