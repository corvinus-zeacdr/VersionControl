using Gyak8_ZEACDR.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyak8_ZEACDR.Entities
{
    public class BallFactory : IToyFactory
    {

        public Color BallColor { get; set; }

        public Toy CreateNew()
        {
        return new Ball(BallColor);
        }

    }
}
