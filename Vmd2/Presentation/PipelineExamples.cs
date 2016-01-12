using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Vmd2.DataAccess;
using Vmd2.Processing;
using Vmd2.Processing.DVR;
using Vmd2.Processing.Filters;
using Vmd2.Processing.Mapping;
using Vmd2.Processing.Segmentation;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation
{
    /// <summary>
    /// Contains prepared example Pipelines
    /// </summary>
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
                    new ProcessingPipeline(
                        "Region Growing",
                        new ProcessingElement[]
                        {
                            new ImageLoader()
                            {
                                Path = TestData.GetPath("vtkBrain")
                            },
                            new RegionGrowing()
                            {
                                DeltaGlobal = 200,
                                DeltaLocal = 20
                            },
                            new DvrRenderer()
                            {
                                TF = new TransferFunction1DBuilder()
                                    .Add(100, Color.FromArgb(0, 0,0,0))
                                    .Add(440, Color.FromArgb(30, 0,0,255))
                                    .Add(560, Color.FromArgb(30, 255,0,0))
                                    .Add(900, Color.FromArgb(0, 0,0,0))
                            }
                        }
                    ),
                    new ProcessingPipeline(
                        "Filtering - WhoBo",
                        new ProcessingElement[]
                        {
                            new ImageLoader()
                            {
                                Path = TestData.GetPath("WholeBodyUncompressed"),
                                CombineFilesToSlices = true
                            },
                            new Slice()
                            {
                                AxisY = true,
                                SliceIndex = 210
                            },
                            new Gaussian2DFilter7x7()
                            {
                                Activated = true
                            },
                            new Laplace2DFilter3x3()
                            {
                                Activated = false
                            },
                            new WindowingRenderer()
                            {
                                WindowCenter = 150,
                                WindowWidth = 300
                            },
                            new MarkerTest()
                            {
                                MarkerX = 300,
                                MarkerY = 560
                            }
                        }
                    ),
                    new ProcessingPipeline(
                        "RegionGrowing - WhoBo",
                        new ProcessingElement[]
                        {
                            new ImageLoader()
                            {
                                Path = TestData.GetPath("WholeBodyUncompressed"),
                                CombineFilesToSlices = true
                            },
                            new TransformYView()
                            {
                                Activated = true
                            },
                            new RegionGrowing()
                            {
                                FilterActivated = false,
                                MarkerX = 300,
                                MarkerY = 560,
                                MarkerZ = 210,
                                DeltaGlobal = 20,
                                DeltaLocal = 10
                            },
                            new DvrRenderer()
                            {
                                TF = new TransferFunction1DBuilder()
                                    .Add(0, Color.FromArgb(0, 255,255,255))
                                    .Add(230, Color.FromArgb(20, 255,10,30))
                                    .Add(400, Color.FromArgb(10, 0,0,0))
                            }
                        }
                    )
                };
        }

        public ProcessingPipeline[] Examples { get; private set; }
    }
}
