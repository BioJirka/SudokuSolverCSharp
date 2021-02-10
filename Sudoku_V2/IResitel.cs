namespace Sudoku {
	// vsechny tridy ResitelSpecialniTyp pochazi z interface IResitel
	// obsahuji funkce NapisZadani a Banuj
	// inicializacni funkce zpravidla vytvori pole Policka (velikosti 9x9)
		// toto pole je zakladem pro funkci Banuj
		// kazdy prvek tohoto pole je list, ktery je tvoren dvojicemi ci trojicemi cisel
		// prvni dve cisla teto dvojice ci trojice jsou souradnice odkazujici na jine policko
			// napr. jsou-li pole ve stejne oblasti, maji-li mezi sebou znamenko
		// treti cislo (existuje-li) uchovava informaci o jejich vzajemne propojenosti
			// napr. ktere je mensi, jaky je jejich rozdil
	// funkce Banuj vzdy pro zadany radek a sloupec projde prislusny list pole Policka a provede prislusne kroky
		// zpravidla banuje nektera cisla v policku, na ktere se odkazuje
	public interface IResitel {
		void NapisZadani(Sudoku sudoku);
		void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo);
	}
}
