using System;

namespace SkladannyaBuletnya.lib
{
    /// <summary>
    /// Утилиты для работы с таблицами.
    /// </summary>
    internal static class TableUtils {

        #region Methods

        /// <summary>
        /// Взятие индекса правой границы в массиве для ключа.
        /// <remarks>Подразумевается что массив отсортирован по возрастанию.</remarks>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Когда ключ <c>key</c> находится вне пределов входного массива <c>input</c>.
        /// </exception>
        /// <param name="input">Входной массив.</param>
        /// <param name="key">Ключ для поиска в массиве.</param>
        /// <returns>Искомый индекс правой границы.</returns>
        public static Int32 GetIndex(Double[] input, Double key) {
            if (key >= input[0]) 
            {
                for (Int32 i = 1; i < input.Length; ++i) {
                    if (input[i] >= key) 
                    {
                        return i;
                    }
                }
            }
            throw new ArgumentOutOfRangeException("Ключ не найден среди входных данных.");
        }

        /// <summary>
        /// Метод ищет во входном массиве ключ и отображает его на выходной массив.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// В случае если ключь <c>key</c> находится вне границ массива <c>input</c>.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// Или если для найденного ключа в массиве выходных данных <c>output</c> нет данных.
        /// </exception>
        /// <param name="input">Масив входных данных.</param>
        /// <param name="key">Ключ.</param>
        /// <param name="output">Массив выходных данных.</param>
        /// <returns>Искомое отображение.</returns>
        public static Double Get(Double[] input, Double key, Double[] output) {
            Int32 i = GetIndex(input, key);
            if (i >= output.Length) {
                throw new IndexOutOfRangeException("Для ключа нет выходных данных");
            }
            return Interpolate(input[i - 1], input[i], key, output[i - 1], output[i]);
        }

        /// <summary>
        /// Поиск отображеня ключа в двумерной таблице.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Генерируется если <c>majorKey</c> не был найден в <c>majorInput</c>,
        /// либо <c>minorKey</c> не был найден в <c>minorInput</c>.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// Когда в таблице <c>data</c> нет данных для входных ключей.
        /// </exception>
        /// <param name="majorInput">Входные данные при рядках.</param>
        /// <param name="majorKey">Ключ для рядка.</param>
        /// <param name="minorInput">Входные данные при столбцах.</param>
        /// <param name="minorKey">Ключ при столбцах.</param>
        /// <param name="data">Таблица.</param>
        /// <returns>Искомое отображение.</returns>
        public static Double StraightGet(Double[] majorInput, Double majorKey, Double[] minorInput, Double minorKey, Double[][] data) {
            try {
                Int32 majorIndex = GetIndex(majorInput, majorKey);
                Double minorLeftOut = Get(minorInput, minorKey, data[majorIndex - 1]),
                    minorRightOut = Get(minorInput, minorKey, data[majorIndex]);
                return Interpolate(majorInput[majorIndex - 1], majorInput[majorIndex], majorKey, minorLeftOut, minorRightOut);
            } catch (ArgumentOutOfRangeException aoe) {
                throw new ArgumentOutOfRangeException("В двухмерной таблице не поддерживаются заданные входные данные.", aoe);
            } catch (IndexOutOfRangeException ioe) {
                throw new IndexOutOfRangeException("В двухмерной таблице не найдены выходные данные для заданных входных.", ioe);
            }
        }

        /// <summary>
        /// Простая интерполяция.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// В случае когда аргумент <c>input</c> лежит вне границ [<c>inputLeft</c>, <c>inputRight</c>].
        /// Если граница <c>inputLeft</c> больше <c>inputRight</c>.
        /// </exception>
        /// <param name="inputLeft">Левая граница входа.</param>
        /// <param name="inputRight">Правая граница входа.</param>
        /// <param name="input">Точка между левой и правой границей входа.</param>
        /// <param name="outputLeft">Левая граница выхода.</param>
        /// <param name="outputRight">Правая граница выхода.</param>
        /// <returns>Точка-отображение между границами выхода пропорционально положению точки входа межджу границами.</returns>
        public static Double Interpolate(Double inputLeft, Double inputRight, Double input, Double outputLeft, Double outputRight) {
            #region Assert
            if (inputLeft > inputRight || input < inputLeft || input > inputRight) {
                throw new ArgumentOutOfRangeException("Неправильный ввод для метода интерполяции.");
            }
            #endregion

            return outputLeft + ((outputRight - outputLeft) * (input - inputLeft)) / (inputRight - inputLeft);
        }

        #endregion

    }

}

/*
 * Сделать поиск бинарным.
 */