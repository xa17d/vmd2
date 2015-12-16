using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Vmd2.DataAccess;
using Vmd2.Processing;
using Vmd2.Processing.DVR;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation
{
    class PipelineExamples
    {
        public PipelineExamples()
        {
            this.Examples =
                new ProcessingPipeline[]
                {
                    new ProcessingPipeline(
                        "Slice TF Rendering",
                        new ProcessingElement[] {
                            new ImageLoader() {
                                Path = TestData.GetPath("vtkBrain")
                            },
                            new Slice(),
                            new TransferFunction1DRenderer()
                        }
                    ),
                    new ProcessingPipeline(
                        "DVR",
                        new ProcessingElement[] {
                            new ImageLoader() {
                                Path = TestData.GetPath("vtkBrain")
                            },
                            new DvrRenderer()
                            {
                                TF = new TransferFunction1DBuilder()
                                    .Add(0, Color.FromArgb(0, 0,0,0))
                                    .Add(380, Color.FromArgb(20, 255,255,255))
                                    .Add(455, Color.FromArgb(30, 0,0,255))
                                    .Add(520, Color.FromArgb(20, 0,0,0))
                                    .Add(1100, Color.FromArgb(0, 0,0,0))
                            }
                        }
                    ),
                };
        }

        public ProcessingPipeline[] Examples { get; private set; }
    }
}
