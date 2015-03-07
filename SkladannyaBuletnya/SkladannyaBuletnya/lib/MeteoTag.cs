namespace SkladannyaBuletnya.lib
{

    using System;
    using lib;
  

    public enum MeteoType { METEONABLIGENIY, METEOSEREDNIY, GOTOVI };
    /// <summary>
    /// Хранит метеоданные.
    /// </summary>
    public class MeteoTag {

        #region Product

        public readonly Double delTauY;

        public readonly Double Wx;

        public readonly Double Wz;

        public readonly Double delH;

       #endregion

        #region Follow

                #endregion

        #region Methods

        private MeteoTag(Table5 table5, Double DH0)
        {
         // Flow: Part (II) and (II б)

            delH = DH0; // 2 (II), 2a (II б)
           
        }// 9 (II), 2а (II б)
                       

        #endregion

        #region Factory

        public MeteoTag()
        {
            
        }

        public static MeteoTag FromMeteoseredniy(String bulletin) {
            #region Validate
            if (bulletin == null) {
                throw new ArgumentNullException("Аргументы не могут быть нулевыми.");
            }
            #endregion

            Double DH0;
            Table5 table5 = Table5.FromMeteoseredniy(bulletin, out DH0); // 1 (II б)
            return new MeteoTag(table5, DH0);
        }

       
        /// <summary>
        /// Создание метео с готовых поправок.
        /// Велика вероятность ошибок.
        /// </summary>
        /// <param name="system"></param>
        /// <param name="stage"></param>
        /// <param name="topo"></param>
        /// <returns></returns>
    
        #endregion


    }
}

/*
 * Добавит описания.
 * Добавить тестов.
 */