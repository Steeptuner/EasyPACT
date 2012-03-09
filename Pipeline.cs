using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyPACT
{
    /// <summary>
    /// Данный класс описывает материал, размеры
    /// и прочие особенности трубопровода произвольного сечения.
    /// </summary>
    class Pipeline
    {
        /// <summary>
        /// ID материала, из которого изготовен трубопровод.
        /// </summary>
        protected int _MaterialId;
        /// <summary>
        /// Длина трубопровода в метрах.
        /// </summary>
        protected double _Length;
        /// <summary>
        /// Эффективный диаметр трубопровода в метрах.
        /// </summary>
        protected double _Diameter;
        /// <summary>
        /// Данный класс описывает материал, размеры
        /// и прочие особенности трубопровода произвольного сечения.
        /// </summary>
        /// <param name="materialId">ID материала, из которого изготовлен трубопровод.</param>
        public Pipeline(int materialId)
        {
            
        }
        /// <summary>
        /// ID материала, из которого изготовен трубопровод.
        /// </summary>
        public int MaterialId
        {
            get { return this._MaterialId; }
            protected set { this.SetMaterial(value); }
        }
        /// <summary>
        /// Длина трубопровода в метрах.
        /// </summary>
        public double Length
        {
            get { return this._Length; }
            protected set { this.SetLength(value); }
        }
        /// <summary>
        /// Эффективный диаметр трубопровода в метрах.
        /// </summary>
        public double Diameter
        {
            get { return this._Diameter; }
        }
        /// <summary>
        /// Задает материал, из которого изготовлен трубопровод.
        /// </summary>
        /// <param name="materialId">ID материала.</param>
        protected void SetMaterial(int MaterialId)
        {
            this._MaterialId = MaterialId;
        }
        /// <summary>
        /// Задает длину трубопровода в метрах.
        /// </summary>
        /// <param name="length">Длина трубопровода в метрах.</param>
        protected void SetLength(double Length)
        {
            this._Length = Length;
        }
        /// <summary>
        /// Задает диаметр трубопровода в метрах.
        /// </summary>
        /// <param name="Square">Площадь поперечного сечения трубопровода в квадратных метрах.</param>
        /// <param name="Perimeter">Величина смоченного периметра в метрах.</param>
        protected virtual void SetDiameter(double Square, double Perimeter)
        {
            this._Diameter = 4 * Square / Perimeter;
        }
    }
}
