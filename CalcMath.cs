using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyPACT
{
    /// <summary>
    /// Предоставляет доступ к методам вычислительной математики
    /// </summary>
    static class CalcMath
    {
        /// <summary>
        /// Производит линейную интерполяцию функции по двум заданным точкам. Возвращает значение функции при заданном аргументе.
        /// </summary>
        /// <param name="point1">Массив из двух элементов, содержащий координаты x, y первой точки.</param>
        /// <param name="point2">Массив из двух элементов, содержащий координаты x, y второй точки.</param>
        /// <param name="x">Значение аргумента, при котором требуется найти значение функции.</param>
        /// <returns>Возвращает значение функции при заданном аргументе.</returns>
        static public double LineatInterpolation(double[] point1, double[] point2, double x)
        {
            return point1[1] + (point2[1] - point1[1]) / (point2[0] - point1[0]) * (x - point1[0]);
        }
    }
}
