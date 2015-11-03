using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Presentation
{
    class ProcessingControl : Attribute
    {
        public ProcessingControl(Type processingElementType)
        {
            this.processingElementType = processingElementType;
        }

        private Type processingElementType;

        public static Type[] FindAllControls()
        {
            List<Type> types = new List<Type>();

            foreach (var type in typeof(ProcessingControl).Assembly.GetTypes())
            {
                var pc = type.GetCustomAttribute<ProcessingControl>();
                if (pc != null)
                {
                    types.Add(pc.processingElementType);
                }
            }

            return types.ToArray();
        }

        public static Type GetControlFromProcessingElement(Type processingElement)
        {
            foreach (var type in typeof(ProcessingControl).Assembly.GetTypes())
            {
                var pc = type.GetCustomAttribute<ProcessingControl>();
                if (pc != null)
                {
                    if (pc.processingElementType == processingElement)
                    {
                        return type;
                    }
                }
            }

            throw new Exception("Control for "+processingElement.Name+" can not be found");
        }
    }
}
