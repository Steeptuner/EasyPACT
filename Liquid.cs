using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EasyPACT
{
    /// <summary>
    /// Данный класс описывает физические свойства 
    /// жидкостей, их зависимости друг от друга
    /// </summary>
    class Liquid
    {
        /// <summary>
        /// Идентификационный номер для поиска в БД
        /// </summary>
        private int _id;
        /// <summary>
        /// Температура жидкости в градусах Цельсия
        /// </summary>
        protected double _Temperature;
        /// <summary>
        /// Давление, оказываемое на жидкость, в Паскалях
        /// </summary>
        protected double _Pressure;
        /// <summary>
        /// Плотность жидкости в килограммах на кубометр
        /// </summary>
        protected double _Density;
        /// <summary>
        /// Динамический коэффициент вязкости жидкости в мПа*с
        /// </summary>
        protected double _ViscosityDynamic;
        /// <summary>
        /// Кинематический коэффициент вязкости жидкости в м2/с
        /// </summary>
        protected double _ViscosityKinematic;
        /// <summary>
        /// Данный класс описывает физические свойства 
        /// жидкостей, их зависимости друг от друга.
        /// </summary>
        /// <param name="id">Идентификационный номер жидкости.</param>
        /// <param name="T">Температура жидкости, в градусах Цельсия.</param>
        /// <param name="P">Давление, оказываемое на жидкость, в Паскалях.</param>
        public Liquid(int id, double T, double P)
        {
            if (id <= 0)
            {
                throw new Exception("Идентификационный номер должен быть положительным числом!");
            }
            if (T <= -273.15)
            {
                throw new Exception("Температура должна быть выше абсолютного нуля!");
            }
            if (P <= 0)
            {
                throw new Exception("Давление должно быть положительным числом!");
            }
            this._id = id;
            this.Temperature = T;
            this.Pressure = P;
        }
        /// <summary>
        /// Возвращает температуру жидкости в градусах Цельсия.
        /// </summary>
        public double Temperature
        {
            get { return this._Temperature; }
            set { this.SetTemperature(value); }
        }
        /// <summary>
        /// Возвращает давление жидкости в Паскалях.
        /// </summary>
        public double Pressure
        {
            get { return this._Pressure; }
            private set { this.SetPressure(value); }
        }
        /// <summary>
        /// Возвращает плотность жидкости при текущей температуре.
        /// </summary>
        public double Density
        {
            get { return this._Density; }
        }
        /// <summary>
        /// Возвращает динамический коэффициент вязкости жидкости при текущей температуре.
        /// </summary>
        public double ViscosityDynamic
        {
            get { return this._ViscosityDynamic; }
        }
        /// <summary>
        /// Возвращает динамический коэффициент вязкости жидкости при текущей температуре.
        /// </summary>
        public double ViscosityKinematic
        {
            get { return this._ViscosityKinematic; }
        }
        /// <summary>
        /// Изменяет температуру жидкости на заданную.
        /// </summary>
        /// <param name="T">Новая температура жидкости в градусах Цельсия.</param>
        private void SetTemperature(double T)
        {
            this._Temperature = T;
            SetDensity();
            SetViscosity();
        }
        /// <summary>
        /// Изменяет давление жидкости на заданное.
        /// </summary>
        /// <param name="T">Новое давление жидкости в Паскалях.</param>
        private void SetPressure(double P)
        {
            this._Pressure = P;
        }
        /// <summary>
        /// Вычисляет плотность жидкости при текущей температуре методом
        /// линейной интерполяции по узловым точкам, имеющимся в базе данных.
        /// </summary>
        protected void SetDensity()
        {
            // Загрузка файла с БД
            XDocument Database = XDocument.Parse(EasyPACT.Properties.Resources.DensityFromTemperature);
            // Список всех точек температура-плотность
            var Points = Database.Root.Elements("substance").Where(a => a.Attribute("id").Value == this._id.ToString()).First().Elements("point");
            // Выбираем минимальную температуру
            var xMin = Points.First().Element("x").Value;
            // Выбираем максимальную температуру
            var xMax = Points.Last().Element("x").Value;
            // Если температура находится за пределами известного интервала, выдать ошибку
            if (this._Temperature < Convert.ToDouble(xMin) || this._Temperature > Convert.ToDouble(xMax))
            {
                throw new Exception("Исключение: Интерполяция невозможна, нет экспериментальных данных в окрестности данной точки!\n");
            }
            // Выбираем известную температуру слева от данной температуры
            var xLeft = Points.Where(a => (Convert.ToDouble(a.Element("x").Value) <= this._Temperature)).Last().Element("x").Value;
            // Выбираем известную температуру справа от данной температуры
            var xRight = Points.Where(a => (Convert.ToDouble(a.Element("x").Value) >= this._Temperature)).First().Element("x").Value;
            // Выбираем плотность при левой температуре
            var yLeft = Points.Where(a => a.Element("x").Value.Equals(xLeft)).First().Element("y").Value;
            // Выбираем плотность при правой температуре
            var yRight = Points.Where(a => a.Element("x").Value.Equals(xRight)).First().Element("y").Value;
            // Если температура совпадает с узловой точкой, то записываем плотность
            if (this._Temperature.ToString() == xLeft)
            {
                this._Density = Convert.ToDouble(yLeft);
                return;
            }
            if (this._Temperature.ToString() == xRight)
            {
                this._Density = Convert.ToDouble(yRight);
                return;
            }
            // Методом линейной интерполяции найдем значение при промежуточной температуре
            double[] point1 = {Convert.ToDouble(xLeft),Convert.ToDouble(yLeft)};
            double[] point2 = {Convert.ToDouble(xRight),Convert.ToDouble(yRight)};
            this._Density = CalcMath.LineatInterpolation(point1, point2, this.Temperature);
        }
        /// <summary>
        /// Вычисляет вязкость при текущей температуре жидкости методом
        /// линейной интерполяции по узловым точкам, имеющимся в базе данных.
        /// </summary>
        protected void SetViscosity()
        {
            // Загрузка файла с БД
            XDocument Database = XDocument.Parse(EasyPACT.Properties.Resources.ViscosityFromTemperature);
            // Список всех точек температура - динамический коэффициент вязкости
            var Points = Database.Root.Elements("substance").Where(a => a.Attribute("id").Value == this._id.ToString()).First().Elements("point");
            // Выбираем минимальную температуру
            var xMin = Points.First().Element("x").Value;
            // Выбираем максимальную температуру
            var xMax = Points.Last().Element("x").Value;
            // Если температура находится за пределами известного интервала, выдать ошибку
            if (this.ViscosityDynamic < Convert.ToDouble(xMin) || this.ViscosityDynamic > Convert.ToDouble(xMax))
            {
                throw new Exception("Исключение: Интерполяция невозможна, нет экспериментальных данных в окрестности данной точки!\n");
            }
            // Выбираем известную температуру слева от данной температуры
            double xLeft = Convert.ToDouble(Points.Where(a => (Convert.ToDouble(a.Element("x").Value) <= this._Temperature)).Last().Element("x").Value);
            // Выбираем известную температуру справа от данной температуры
            double xRight = Convert.ToDouble(Points.Where(a => (Convert.ToDouble(a.Element("x").Value) >= this._Temperature)).First().Element("x").Value);
            // Выбираем динамический коэффициент вязкости при левой температуре
            double yLeft = Convert.ToDouble(
                Points
                .Where(a => Convert.ToDouble(a.Element("x").Value) == xLeft)
                .First()
                .Element("y")
                .Value);
            // Выбираем динамический коэффициент вязкости при правой температуре
            double yRight = Convert.ToDouble(
                Points
                .Where(a => Convert.ToDouble(a.Element("x").Value) == xRight)
                .First()
                .Element("y")
                .Value);
            // Если температура совпадает с узловой точкой, то записываем динамический коэффициент вязкости
            if (this.Temperature == xLeft)
            {
                this._ViscosityDynamic = yLeft;
                this._ViscosityKinematic = this.ViscosityDynamic / 1000 / this.Density;
                return;
            }
            if (this.Temperature == xRight)
            {
                this._ViscosityDynamic = yRight;
                this._ViscosityKinematic = this.ViscosityDynamic / 1000 / this.Density;
                return;
            }
            // Методом линейной интерполяции найдем значение при промежуточной температуре
            double[] point1 = { xLeft, yLeft };
            double[] point2 = { xRight, yRight };
            this._ViscosityDynamic = CalcMath.LineatInterpolation(point1, point2, this.ViscosityDynamic);
            // Вычислим кинематическую вязкость
            this._ViscosityKinematic = this.ViscosityDynamic / 1000 / this.Density;
        }
    }
}
