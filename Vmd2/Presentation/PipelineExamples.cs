using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                            }
                        }
                    ),
                };
        }
        
        public ProcessingPipeline[] Examples { get; private set; }
    }
}
