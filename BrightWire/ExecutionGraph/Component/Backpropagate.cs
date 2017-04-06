﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrightWire.ExecutionGraph.Component
{
    class Backpropagate : IComponent
    {
        readonly IErrorMetric _errorMetric;

        public Backpropagate(IErrorMetric errorMetric)
        {
            _errorMetric = errorMetric;
        }

        public void Dispose()
        {
            // nop
        }

        public IMatrix Execute(IMatrix input, IBatchContext context)
        {
            context.SetOutput(input);
            return input;
        }

        public IMatrix Train(IMatrix input, int channel, IBatchContext context)
        {
            context.SetOutput(input);
            var gradient = _errorMetric.CalculateGradient(input, context.Target);
            context.CalculateTrainingError(gradient);
            context.Backpropagate(gradient, channel);
            return Execute(input, context);
        }
    }
}
