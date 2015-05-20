using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using OfficeOpenXml;
using SkladannyaBuletnya.Properties;
using SkladannyaBuletnya.lib;
using libxl;


namespace SkladannyaBuletnya.lib
{
    class RVDS
    {
        const double PI = 3.1415926535897932384626433832795;
        
        void Calculation(int IS, int VC, int Dt, double At, int Nz, double Tz, int dV0, int hb, int B, double Dv, double Av)
        {

	if (IS<1 || IS>8) return; //Проверка входящего параметра IS
	if (VC<1 || VC>4) return; //Проверка входящего параметра VC
	if (Nz<0 || Nz>6) return; //Проверка Nz
	if (Dt<0) return; //Проверка Dt
	if (B>90 || B < 0) return;


	Book book = new BinBook(); // use XmlBook() for xlsx
    Sheet sheet = book.addSheet("");

	double dVoTz = 0;

	double gZ = 0;
	double gZw = 0;
	double gXw = 0;
	double gXT = 0;
	double gXV0 = 0;

	double dZgf = 0;
	double dXgf = 0;

	double Z = 0;
	double dZw = 0;
	double dXw = 0;
	double dXH = 0;
	double dXHH = 0;
	double dXT = 0;
	double dXV0 = 0;
	double Yb = 0;
	double Bd = 0;

	double dXTz = 0;

	if (IS <= 7) {  //IS<=7
        book = new BinBook();																										
		book.load ("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_Vo_TZ.xls");																						
		sheet = book.getSheet(0);																									
		int i=0;																													
		i=findRow(Tz, 4, 0, sheet, false);																							
		dVoTz= interpolation(Tz, sheet.readNum(i, 0), sheet.readNum(i+1, 0), sheet.readNum(i, Nz+1), sheet.readNum(i+1, Nz+1));
        if (hb > 500){ //Поиск в таблицах при hb>500
			if (IS <= 5){
				if (VC < 4){
                    book = new BinBook();
					book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_2.9_G.xls");																						
					sheet = book.getSheet(Nz);																					
					i = 0;																													
					i = findNearestRow(Dt, 4, 0, sheet, true);
					gZ = sheet.readNum(i, 1);
					gZw = sheet.readNum(i, 2);
					gXw = sheet.readNum(i, 3);
					gXT = sheet.readNum(i, 4);
					gXV0 = sheet.readNum(i, 5);
				}
				else {
                    book = new BinBook();
					book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_2.9_GM.xls");
					sheet = book.getSheet(Nz);
					i = 0;
					i = findNearestRow(Dt, 4, 0, sheet, false);
					gZ = sheet.readNum(i, 1);
					gZw = sheet.readNum(i, 2);
					gXw = sheet.readNum(i, 3);
					gXT = sheet.readNum(i, 4);
					gXV0 = sheet.readNum(i, 5);
					 }
			}
			else{ //IS 6,7
                book = new BinBook();
				book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_2.9_СG.xls");
				sheet = book.getSheet(Nz);
				i = 0;
				i = findNearestRow(Dt, 4, 0, sheet, true);
				gZ = sheet.readNum(i, 1);
				gZw = sheet.readNum(i, 2);
				gXw = sheet.readNum(i, 3);
				gXT = sheet.readNum(i, 4);
				gXV0 = sheet.readNum(i, 5);
			}
		}

        book = new BinBook();																										
		book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_2.8_Go.xls");																						
		sheet = book.getSheet(Nz);
		i = findNearestRow(Dt, 9, 0, sheet, true); 
		int k = FindCellAndCardinalDirection(At, B); //Здесь не делал интерполяцию так как из-за того что зависимость значений таблицы от B не линейные. Бралось самое близкое значение
		dXgf = sheet.readNum(i, k); 
		i = findNearestRow(Dt, 45, 0, sheet, true);
		dZgf = sheet.readNum(i, k);
	}

	double Di = Dt;

	do{

		if (IS <= 5){
			if (VC <= 3){
                book = new BinBook();
				book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_2.4.xls");
				sheet = book.getSheet(Nz*2);
				int i = 0;
				i = findRow(Di, 10, 0, sheet, true);
				Z = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 4), sheet.readNum(i + 1, 4));
				dZw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 5), sheet.readNum(i + 1, 5));
				dXw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 6), sheet.readNum(i + 1, 6));
				dXH = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 7), sheet.readNum(i + 1, 7));
				dXHH = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 8), sheet.readNum(i + 1, 8));
				dXT = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 9), sheet.readNum(i + 1, 9));
				dXV0 = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 10), sheet.readNum(i + 1, 10));
				Yb = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 13), sheet.readNum(i + 1, 13));
				Bd = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 3), sheet.readNum(i + 1, 3));
			}
			else {
                book = new BinBook();
				book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_2.4.xls");
				sheet = book.getSheet(Nz * 2+1);
				int i = 0;
				i = findRow(Di, 10, 0, sheet, true);
				Z = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 4), sheet.readNum(i + 1, 4));
				dZw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 5), sheet.readNum(i + 1, 5));
				dXw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 6), sheet.readNum(i + 1, 6));
				dXH = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 7), sheet.readNum(i + 1, 7));
				dXHH = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 8), sheet.readNum(i + 1, 8));
				dXT = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 9), sheet.readNum(i + 1, 9));
				dXV0 = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 10), sheet.readNum(i + 1, 10));
				Yb = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 13), sheet.readNum(i + 1, 13));
				Bd = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 3), sheet.readNum(i + 1, 3));
				
			}
		}
		if (IS == 6 || IS == 7){
            book = new BinBook();
			book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_2.6.xls");
			sheet = book.getSheet(Nz);
			int i = 0;
			i = findRow(Di, 10, 0, sheet, true);
			Z = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 10), sheet.readNum(i + 1, 10));
			dZw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 11), sheet.readNum(i + 1, 11));
			dXw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 12), sheet.readNum(i + 1, 12));
			dXH = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 13), sheet.readNum(i + 1, 13));
			dXHH = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 14), sheet.readNum(i + 1, 14));
			dXT = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 15), sheet.readNum(i + 1, 15));
			dXV0 = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 16), sheet.readNum(i + 1, 16));
			Yb = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 19), sheet.readNum(i + 1, 19));
			Bd = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 8), sheet.readNum(i + 1, 8));
			
		}
		if (IS == 8){
            book = new BinBook();
			book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_22.xls");
			sheet = book.getSheet(0);
			int i = 0;
			i = findRow(Di, 12, 0, sheet, true);
			Z = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 7), sheet.readNum(i + 1, 7));
			dZw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 8), sheet.readNum(i + 1, 8));
			dXw = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 9), sheet.readNum(i + 1, 9));
			dXH = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 10), sheet.readNum(i + 1, 10));
			dXT = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 11), sheet.readNum(i + 1, 11));
			dXTz = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 12), sheet.readNum(i + 1, 12));
			dXV0 = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 13), sheet.readNum(i + 1, 13));
			Yb = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 19), sheet.readNum(i + 1, 19));
			Bd = interpolation(Di, sheet.readNum(i, 0), sheet.readNum(i + 1, 0), sheet.readNum(i, 4), sheet.readNum(i + 1, 4));
			
		}

		//////////////////////////////////////////////
		//Здесь будет подпрограма METEO, а пока что://
		double hm = 100;						    //
		double dHm = 20;							//
		double dT = 10;								//
		double Lw = 20.00;							//
		double W = 7;								//
		//////////////////////////////////////////////
		if (At < Lw)At = At + 60;

		//подпрограма визначення барометричного ступеня//
        book = book = new BinBook();
		book.load("D:\\Study\\Воєнна кафедра\\git\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\tabl_bh.xls");
		sheet = book.getSheet(0);
		int i1 = 0;
		i1 = findNearestRow(dHm, 2, 0, sheet, false); 
		int k = 0;
		k = findNearestCell(dT, 1, 1, sheet, false);
		double Bh = sheet.readNum(i1, k);
		

		double dH = dHm + (hm - hb) / Bh;
		double Aw = (At - Lw) * PI / 30;
		double Wx = -Math.Cos(Aw) * W;
		double Wz = Math.Sin(Aw) * W;

		//Конечные вычисления
		double dZs;
		double dXs;
		if (IS == 8){
			double dTz = Tz - 15;
			dZs = Z + 0.1*dZw*Wz;
			dXs = 0.1*dXw*Wx + 0.1*dXH*dH + 0.1*dXT*dT+0.1*dXTz*dTz+dXV0*dV0;
		}
		else {
			double dV0s = dV0 + dVoTz;
			dZs = Z + hb / 1000 * gZ + 0.1*(dZw + hb / 1000 * gZw)*Wz + dZgf;
			dXs = 0.1*(dXw + hb / 1000 * gXw)*Wx + 0.1*(dXH + 0.1*dXHH*dH)*dH + 0.1*(dXT + hb / 1000 * gXT)*dT + (dXV0 + hb / 1000 * gXV0)*dV0s + dXgf;
		}
		double LastDi = Di;
		Di = Dt + dXs;
		if (Math.Abs(Di - LastDi) < Bd){
			Dv = Dt + dXs;
		    Av = At + dZs/100;
			while (Av > 60){
				Av = Av - 60;
			}
			break;
		}
	} while (true);
}

        //Интерполяция
        double interpolation(double k, double x1, double x2, double y1, double y2)
        {
            double p1 = x1 - x2;
            double p2 = y1 - y2;
            double percent = p2 / p1;
            double diff = k - x1;
            return y1 + (diff * percent);
        }
        //Поиск ячейки которая ближе к даному значению
        int findNearestRow(double k, int StartPosRow, int StartPosCell, libxl.Sheet sheet, bool increase){
	    int i = 0;
	    if (increase){
		while (sheet.readNum(StartPosRow + i + 1, StartPosCell)<k){
			if (libxl.CellType.CELLTYPE_EMPTY == sheet.cellType(StartPosRow + i + 2, StartPosCell)) break;
			i++;
		}
	}
	else {
		while (sheet.readNum(StartPosRow + i + 1, StartPosCell)>k){
            if (libxl.CellType.CELLTYPE_EMPTY == sheet.cellType(StartPosRow + i + 2, StartPosCell)) break;
			i++;
		}
	}
	double j = sheet.readNum(StartPosRow + i, StartPosCell);
	double l = sheet.readNum(StartPosRow + i + 1, StartPosCell);
	if (Math.Abs(j - k) >= Math.Abs(l - k)) i++;
	return StartPosRow + i;
}

        //Поиск ячейки которая ближе к даному значению
        int findNearestCell(double k, int StartPosRow, int StartPosCell, libxl.Sheet sheet, bool increase){
	int i = 0;
	if (increase){
		while (sheet.readNum(StartPosRow, StartPosCell + i + 1)<k){
			if (libxl.CellType.CELLTYPE_EMPTY == sheet.cellType(StartPosRow, StartPosCell + i + 2)) break;
			i++;
		}
	}
	else {
		while (sheet.readNum(StartPosRow, StartPosCell + i + 1)>k){
            if (libxl.CellType.CELLTYPE_EMPTY == sheet.cellType(StartPosRow, StartPosCell + i + 2)) break;
			i++;
		}
	}
	double j = sheet.readNum(StartPosRow, StartPosCell + i);
	double l = sheet.readNum(StartPosRow, StartPosCell + i + 1);
	if (Math.Abs(j - k) >= Math.Abs(l - k)) i++;
	return StartPosCell + i;
}

        //Поиск ячеек между какими находится число. Первая ячейка возвращаемое значение, а вторая -- значение+1
        int findRow(double k, int StartPosRow, int StartPosCell, libxl.Sheet sheet, bool increase){
	int i = 0;
	if (increase){
		while (sheet.readNum(StartPosRow + i + 1, StartPosCell)<k){
            if (libxl.CellType.CELLTYPE_EMPTY == sheet.cellType(StartPosRow + i + 2, StartPosCell)) break;
			i++;
		}
	}
	else {
		while (sheet.readNum(StartPosRow + i+1, StartPosCell)>k){
			if (libxl.CellType.CELLTYPE_EMPTY == sheet.cellType(StartPosRow + i+2, StartPosCell)) break;
			i++;
		}
	}
	return StartPosRow + i;
}
        // Выдает индекс столбца с tabl_2.8_Go за At и B
        int FindCellAndCardinalDirection(double At, double B)
        {
            double at = At;
            while (at < 0)
            {
                at = at + 60;
            }
            while (at >= 60)
            {
                at = at - 60;
            }
            int k = 0;
            //Приходится делать так. Ибо таблица неудобная
            if (at >= 56.25 || at < 3.75) k = 0;
            if (at >= 3.75 && at < 11.25) k = 1;
            if (at >= 11.25 && at < 18.75) k = 2;
            if (at >= 18.75 && at < 25.75) k = 3;
            if (at >= 25.75 && at < 33.25) k = 4;
            if (at >= 33.25 && at < 40.75) k = 5;
            if (at >= 40.75 && at < 49.25) k = 6;
            if (at >= 49.25 && at < 56.25) k = 7;
            int b = 0;
            if (B >= 0 && B < 20) b = 0;
            if (B >= 20 && B < 40) b = 1;
            if (B >= 40 && B < 60) b = 2;
            if (B >= 60 && B <= 90) b = 3;
            return k * 4 + 1 + b;
        }
    }
}