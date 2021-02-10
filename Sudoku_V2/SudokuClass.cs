using System;
using System.Collections.Generic;

namespace Sudoku {
	// trida Sudoku je hlavni trida obsahujici radu mensichc trid
	public class Sudoku {
		public int Zarazka;
		public int ZarazkaStrop;
		public int RozmerCisla;
		public int RozmerMrizka;
		public UlohaSudoku Uloha;
		public Dalsi DalsiKrok;
		public Policko[,] Policka;
		public int KrokyPocet;
		public List<Krok> Kroky;
		public int HodnotaMax;
		public int HodnotaCislo;
		public bool FlagResime;
		public int PocetReseni;
		public IList<IResitel> Resitele;
		public Sudoku(UlohaSudoku uloha) {
			this.Zarazka = 0;
			this.ZarazkaStrop = 100000;
			this.RozmerCisla = uloha.RozmerCisla;
			this.RozmerMrizka = uloha.RozmerMrizka;
			this.Uloha = uloha;
			this.Policka = new Policko[this.RozmerMrizka, this.RozmerMrizka];
			for (int i = 0; i < this.RozmerMrizka; i++) {
				for (int j = 0; j < this.RozmerMrizka; j++) {
					this.Policka[i, j] = new Policko(this.RozmerCisla);
				}
			}
			this.KrokyPocet = 0;
			this.Kroky = new List<Krok>();
			this.DalsiKrok = new Dalsi();
			this.HodnotaMax = 1000000;
			this.HodnotaCislo = 1000;
			this.Resitele = new List<IResitel>();
			// pri inicializaci si trida (dle typu sudoku) nacte potrebne tridy ResitelSpecialniType
			NactiResitele();
			// nacte vstupni cisla
			NactiCisla();
			// provede zakladni ohodnoceni poli, aby vedela, na kterem poli ma zacit lustit
			OhodnotVse();
			// urci pole, na kterem se zacne lustit
			UrciDalsiPole();
		}
		// nacte potrebne resitele
		public void NactiResitele() {
			Resitele.Add(new ResitelSudoku(this));
			if (Uloha.Typ.Contains("Basic")) {
				Resitele.Add(new ResitelBasic(this));
			}
			if (Uloha.Typ.Contains("Diagonal")) {
				Resitele.Add(new ResitelDiagonal(this));
			}
			if (Uloha.Typ.Contains("Windoku")) {
				Resitele.Add(new ResitelWindoku(this));
			}
			if (Uloha.Typ.Contains("Region")) {
				Resitele.Add(new ResitelRegion(this));
			}
			if (Uloha.Typ.Contains("Suda")) {
				Resitele.Add(new ResitelSuda(this));
			}
			if (Uloha.Typ.Contains("Licha")) {
				Resitele.Add(new ResitelLicha(this));
			}
			if (Uloha.Typ.Contains("AntiJezdec")) {
				Resitele.Add(new ResitelAntiJezdec(this));
			}
			if (Uloha.Typ.Contains("Pevnost")) {
				Resitele.Add(new ResitelPevnost(this));
			}
			if (Uloha.Typ.Contains("Nerovnost")) {
				Resitele.Add(new ResitelNerovnost(this));
			}
			if (Uloha.Typ.Contains("Teplomer")) {
				Resitele.Add(new ResitelTeplomer(this));
			}
			if (Uloha.Typ.Contains("Sousledna")) {
				Resitele.Add(new ResitelSousledna(this));
			}
			if (Uloha.Typ.Contains("Rimska")) {
				Resitele.Add(new ResitelRimska(this));
			}
			if (Uloha.Typ.Contains("Rozdil")) {
				Resitele.Add(new ResitelRozdil(this));
			}
			if (Uloha.Typ.Contains("KdeJe9")) {
				Resitele.Add(new ResitelKdeJe9(this));
			}
		}
		// nacte cisla ze zadani
		// zaroven vola funkci Banuj
		public void NactiCisla() {
			for (int i = 0; i < this.RozmerMrizka; i++) {
				for (int j = 0; j < this.RozmerMrizka; j++) {
					if (this.Uloha.Cisla[i][j] != "") {
						this.Policka[i, j].Cislo = Convert.ToInt32(this.Uloha.Cisla[i][j]);
						Banuj(i + 1, j + 1, Convert.ToInt32(this.Uloha.Cisla[i][j]));
					}
				}
			}
		}
		// pokud je policko vyplneno, zada 0
		// jinak pokud do policka nelze zapsat zadne cislo, zada max. hodnotu
		// jinak zada hodnotu dle poctu banu a bonusu policka
		public void Ohodnot(Policko policko) {
			if (policko.Cislo != 0) {
				policko.Hodnota = 0;
			} else if (policko.BanyPocet == this.RozmerCisla) {
				policko.Hodnota = HodnotaMax;
			} else {
				policko.Hodnota = 1000 * policko.BanyPocet + policko.HodnotaBonus;
			}
		}
		public void OhodnotVse() {
			for (int i = 0; i < this.RozmerMrizka; i++) {
				for (int j = 0; j < this.RozmerMrizka; j++) {
					Ohodnot(this.Policka[i, j]);
				}
			}
		}
		// do tridy DalsiKrok si zapise pole s nejvyssi hodnotou
		public void UrciDalsiPole() {
			DalsiKrok.Hodnota = 0;
			for (int i = 0; i < this.RozmerMrizka; i++) {
				for (int j = 0; j < this.RozmerMrizka; j++) {
					if (Policka[i, j].Hodnota > DalsiKrok.Hodnota) {
						DalsiKrok.Radek = i + 1;
						DalsiKrok.Sloupec = j + 1;
						DalsiKrok.Hodnota = Policka[i, j].Hodnota;
					}
				}
			}
		}
		// tato funkce by se dala prohlasit za jadro programu
		// nastavuje indikator, ktery udava dalsi chovani programu
			// 2 - sudoku je kompletne vyplneno
			// 1 - v dalsim kroku budeme zapisovat cislo
			// 0 - v dalsim kroku budeme menit posledne zapsane cislo
			// -1 - v dalsim kroku budeme mazat posledni zapsane cislo
			// -2 - prosli jsme vsechny moznosti, konec programu
		public void UrciDalsiKrok() {
			switch (DalsiKrok.Indikator) {
				// pokud predchozi krok byl 'vyplneno'
				case 2:
					// pokud posledni zapsane cislo mohu zmenit na jine
					if (Kroky[KrokyPocet - 1].CisloPoradi + 1 < Kroky[KrokyPocet - 1].MoznaCisla.Count) {
						DalsiKrok.Indikator = 0;
					} else {
						DalsiKrok.Indikator = -1;
					}
					break;
				// pokud predchozi krok byl 'zapsat cislo'
				case 1:
					// pokud nedoslo k erroru a vsechna policka vyplnena
					if (!DalsiKrok.Error && DalsiKrok.Hodnota == 0) {
						DalsiKrok.Indikator = 2;
					// pokud nedoslo k erroru a do vsech nevyplnenych policek lze zapsat alespon 1 cislo
					} else if (!DalsiKrok.Error && DalsiKrok.Hodnota < HodnotaMax) {
						DalsiKrok.Indikator = 1;
					// pokud posledni zapsane cislo mohu zmenit na jine
					} else if (Kroky[KrokyPocet - 1].CisloPoradi + 1 < Kroky[KrokyPocet - 1].MoznaCisla.Count) {
						DalsiKrok.Indikator = 0;
					} else {
						DalsiKrok.Indikator = -1;
					}
					break;
				// pokud predchozi krok byl 'zmenit cislo'
				case 0:
					// pokud nedoslo k erroru a vsechna policka vyplnena
					if (!DalsiKrok.Error && DalsiKrok.Hodnota == 0) {
						DalsiKrok.Indikator = 2;
					// pokud nedoslo k erroru a do vsech nevyplnenych policek lze zapsat alespon 1 cislo
					} else if (!DalsiKrok.Error && DalsiKrok.Hodnota < HodnotaMax) {
						DalsiKrok.Indikator = 1;
					// pokud posledni zapsane cislo mohu zmenit na jine
					} else if (Kroky[KrokyPocet - 1].CisloPoradi + 1 < Kroky[KrokyPocet - 1].MoznaCisla.Count) {
						DalsiKrok.Indikator = 0;
					} else {
						DalsiKrok.Indikator = -1;
					}
					break;
				// pokud predchozi krok byl 'smazat cislo'
				case -1:
					// pokud zadne cislo nelze zmenit ani smazat
					if (KrokyPocet == 0) {
						DalsiKrok.Indikator = -2;
					// pokud posledni zapsane cislo mohu zmenit na jine
					} else if (Kroky[KrokyPocet - 1].CisloPoradi + 1 < Kroky[KrokyPocet - 1].MoznaCisla.Count) {
						DalsiKrok.Indikator = 0;
					} else {
						DalsiKrok.Indikator = -1;
					}
					break;
			}
			DalsiKrok.Error = false;
		}
		// na zaklade indikatoru provede dalsi krok
		// nasledne urci dalsi pole a dalsi krok
		public void UdelejDalsiKrok() {
			switch (DalsiKrok.Indikator) {
				case 2:
					NapisReseni();
					break;
				case 1:
					NapisCislo();
					break;
				case 0:
					ZmenCislo();
					break;
				case -1:
					SmazCislo();
					break;
			}
			UrciDalsiPole();
			UrciDalsiKrok();
		}
		public void NapisCislo() {
			// vytvori novou polozku v listu Kroky
			Kroky.Add(new Krok(DalsiKrok.Radek, DalsiKrok.Sloupec));
			KrokyPocet += 1;
			// pro dane policko si vytvorime seznam moznych cisel na vepsani
			for (int i = 0; i < this.RozmerCisla; i++) {
				if (!Policka[DalsiKrok.Radek - 1, DalsiKrok.Sloupec - 1].Bany[i]) {
					Kroky[KrokyPocet - 1].MoznaCisla.Add(i + 1);
				}
			}
			// zapise prvni z moznych cisel
			Policka[DalsiKrok.Radek - 1, DalsiKrok.Sloupec - 1].Cislo = Kroky[KrokyPocet - 1].MoznaCisla[0];
			// spusti banovaci proces
			Banuj(DalsiKrok.Radek, DalsiKrok.Sloupec, Policka[DalsiKrok.Radek - 1, DalsiKrok.Sloupec - 1].Cislo);
		}
		public void ZmenCislo() {
			// zrusi vsechny bany, ktere byly na zaklade posledniho kroku provedeny
			OdBanuj();
			Kroky[KrokyPocet - 1].CisloPoradi += 1;
			// zapise nove cislo
			Policka[Kroky[KrokyPocet - 1].Radek - 1, Kroky[KrokyPocet - 1].Sloupec - 1].Cislo = Kroky[KrokyPocet - 1].MoznaCisla[Kroky[KrokyPocet -1].CisloPoradi];
			// spusti banovaci proces
			Banuj(Kroky[KrokyPocet - 1].Radek, Kroky[KrokyPocet - 1].Sloupec, Policka[Kroky[KrokyPocet - 1].Radek - 1, Kroky[KrokyPocet - 1].Sloupec - 1].Cislo);
		}
		public void SmazCislo() {
			// cislo nastavime na 0
			Policka[Kroky[KrokyPocet - 1].Radek - 1, Kroky[KrokyPocet - 1].Sloupec - 1].Cislo = 0;
			// zrusime bany
			OdBanuj();
			// smazeme posledni krok
			Kroky.RemoveAt(KrokyPocet - 1);
			KrokyPocet -= 1;
		}
		// vola funkce Banuj jednotlivych trid ResitelSpecialniTyp
		public void Banuj(int radek, int sloupec, int cislo) {
			foreach (var resitel in Resitele) {
				resitel.Banuj(this, radek, sloupec, cislo);
			}
		}
		// tato funkce je volana funkcemi Banuj ze trid ResitelSpecialniTyp
		public void BanujPolicko(int radek, int sloupec, int cislo) {
			// pokud ban jeste neexistuje
			if (!Policka[radek - 1, sloupec - 1].Bany[cislo - 1]) {
				// zapis ban
				Policka[radek - 1, sloupec - 1].Bany[cislo - 1] = true;
				Policka[radek - 1, sloupec - 1].BanyPocet += 1;
				// znovu ohodnot policko
				// jeho hodnota a tudiz priorita pri reseni se zvysi s kazdym banem
				Ohodnot(Policka[radek - 1, sloupec - 1]);
				// pokud jsme ve fazi reseni sudoku, pak zapiseme provedeny ban
				if (FlagResime) {
					Kroky[KrokyPocet - 1].KrokBany.Add(new KrokBan(radek, sloupec, cislo));
				}
			}
		}
		// tato funkce je volana funkcemi Banuj ze trid ResitelSpecialniTyp
		// tato funkce provede opak, nez funkce BanujPolicko, banuje vsechna cisla vyjma zadaneho
		public void BanujPolickoInverse(int radek, int sloupec, int cislo) {
			for (int i = 0; i < this.RozmerCisla; i++) {
				if (!Policka[radek - 1, sloupec - 1].Bany[i] && i + 1 != cislo) {
					Policka[radek - 1, sloupec - 1].Bany[i] = true;
					Policka[radek - 1, sloupec - 1].BanyPocet += 1;
					if (FlagResime) {
						Kroky[KrokyPocet - 1].KrokBany.Add(new KrokBan(radek, sloupec, i + 1));
					}
				}
			}
			Ohodnot(Policka[radek - 1, sloupec - 1]);
		}
		// tato funkce je volana funkcemi Banuj ze trid ResitelSpecialniTyp
		// obdobna funkce jako funkce BanujPolickoInverse, ale banuje vsechna cisla vyjma 2 zadanych
		public void BanujPolickoInverse_2(int radek, int sloupec, int cislo_1, int cislo_2) {
			for (int i = 0; i < this.RozmerCisla; i++) {
				if (!Policka[radek - 1, sloupec - 1].Bany[i] && i + 1 != cislo_1 && i + 1 != cislo_2) {
					Policka[radek - 1, sloupec - 1].Bany[i] = true;
					Policka[radek - 1, sloupec - 1].BanyPocet += 1;
					if (FlagResime) {
						Kroky[KrokyPocet - 1].KrokBany.Add(new KrokBan(radek, sloupec, i + 1));
					}
				}
			}
			Ohodnot(Policka[radek - 1, sloupec - 1]);
		}
		// projde vsechny bany udelane v poslednim kroku a zrusi je
		public void OdBanuj() {
			foreach (var unBan in Kroky[KrokyPocet - 1].KrokBany) {
				Policka[unBan.Radek - 1, unBan.Sloupec - 1].Bany[unBan.Cislo - 1] = false;
				Policka[unBan.Radek - 1, unBan.Sloupec - 1].BanyPocet -= 1;
				Ohodnot(Policka[unBan.Radek - 1, unBan.Sloupec - 1]);
			}
			// smaze frontu banu z posledniho kroku
			Kroky[KrokyPocet - 1].KrokBany = new List<KrokBan>();
		}
		// zavola vsechny resitele, aby si vypsali prislusne kusy vstupu
		public void NapisZadani() {
			foreach (var resitel in Resitele) {
				resitel.NapisZadani(this);
			}
		}
		public void NapisReseni() {
			Console.WriteLine("Sudoku reseni (pocet kroku {0}):", Zarazka);
			if (this.RozmerMrizka == 9) {
				for (int i = 0; i < 9; i++) {
					if (i % 3 == 0) {
						Console.WriteLine("-------------");
					}
					for (int j = 0; j < 9; j++) {
						if (j % 3 == 0) {
							Console.Write('|');
						}
						if (Policka[i, j].Cislo == 0) {
							Console.Write(' ');
						} else {
							Console.Write(Policka[i, j].Cislo);
						}
					}
					Console.WriteLine('|');
				}
				Console.WriteLine("-------------");
			} else {
				for (int i = 0; i < this.RozmerMrizka; i++) {
					Console.Write('-');
				}
				Console.WriteLine("--");
				for (int i = 0; i < this.RozmerMrizka; i++) {
					Console.Write('|');
					for (int j = 0; j < this.RozmerMrizka; j++) {
						if (Policka[i, j].Cislo == 0) {
							Console.Write(' ');
						} else {
							Console.Write(Policka[i, j].Cislo);
						}
					}
					Console.WriteLine('|');
				}
				for (int i = 0; i < this.RozmerMrizka; i++) {
					Console.Write('-');
				}
				Console.WriteLine("--");
			}
			Console.WriteLine();
			this.PocetReseni += 1;
		}
		// nepouziva se pro beh programu, ale pro potreby debugovani
		public void NapisBany(int cislo) {
			Console.WriteLine("Sudoku bany {0}:", cislo);
			if (this.RozmerMrizka == 9) {
				for (int i = 0; i < 9; i++) {
					if (i % 3 == 0) {
						Console.WriteLine("-------------");
					}
					for (int j = 0; j < 9; j++) {
						if (j % 3 == 0) {
							Console.Write('|');
						}
						if (Policka[i, j].Cislo == cislo) {
							Console.Write(cislo);
						} else if (Policka[i, j].Bany[cislo - 1]) {
							Console.Write('X');
						} else {
							Console.Write(' ');
						}
					}
					Console.WriteLine('|');
				}
				Console.WriteLine("-------------");
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine();
			}
		}
		// hlavni funkce
		public void VyresSudoku() {
			FlagResime = true;
			NapisZadani();
			UrciDalsiKrok();
			// vyresi sudoku
			while (Zarazka < ZarazkaStrop && DalsiKrok.Indikator != -2) {
				UdelejDalsiKrok();
				Zarazka += 1;
			}
			// vypise finalni komentar
			if (this.PocetReseni == 0) {
				if (Zarazka == ZarazkaStrop) {
					Console.WriteLine("Je nam lito, ale sudoku se na {0} kroku nepodarilo vylustit...", ZarazkaStrop);
				} else {
					Console.WriteLine("Je nam lito, ale toto sudoku nema reseni, provedeno {0} kroku...", Zarazka);
				}
			} else {
				if (Zarazka == ZarazkaStrop) {
					Console.WriteLine("Reseni nalezena na {0} kroku uvedena vyse, mohou existovat dalsi...", ZarazkaStrop);
				} else {
					Console.WriteLine("Vsechna reseni byla na {0} kroku nalezena...", Zarazka);
				}
			}
		}
		// nepouziva se pro beh programu, ale pro potreby debugovani
		public void VyresSudokuPomalu(int krokujOd, int pocetKroku, int velikostKroku) {
			FlagResime = true;
			NapisReseni();
			UrciDalsiKrok();
			while (Zarazka < krokujOd + pocetKroku) {
				UdelejDalsiKrok();
				Zarazka += 1;
				if (Zarazka >= krokujOd && (Zarazka - krokujOd) % velikostKroku == 0) {
					NapisReseni();
				}
			}
		}
	}
}
