// popis logiky programu
// dle typu sudoku (typy lze libovolne kombinovat) si hlavni trida Sudoku vytvori list s podtridami ResitelSpecialniTyp
// tyto tridy bude pozdeji volat
// trida Sudoku si udrzuje informace o tom, kolik ruznych cisel lze do jakeho pole zapsat
// po kazdem kroku se rozhoduje, jak budeme dale postupovat:
	// pokud existuje prazdne pole, kam nelze zapsat zadne cislo, narazili jsme na slepou vetev a musime se vratit
		// zkusime zmenit posledne zapsane cislo, pokud nelze, pak jej smazeme
	// pokud jsme mazali, nesmi byt dalsi krok zapsani cisla, jinak se zacyklime
	// ve zbyvajicich pripadech zapiseme dalsi cislo
// je navrzena heuristika, kdy nejprve vyplnujeme ta pole, do kterych lze zapsat nejnizsi pocet cisel
	// v pripade slepych vetvi tak nemusime prochazet tolik moznosti
// tato heuristika je upravena pomoci prvku HodnotaBonus v podtridach Policko
	// je to proto, ze u specializovanych typu sudoku chceme vyplnovat pole se specialnimi vlastnostmi nebo informacemi jako prvni
// popis jednotlivych druhu sudoku viz tridy ResitelSpecialniTyp
// popis pridani nove tridy ResitelSpecialniTyp
	// Sudoku Solver jako takovy je vlastne hotov, je mozne jej rozsirit o dalsi typy specializovanych sudoku
	// rozsirit o novy typ sudoku znamena pridat novou tridu ResitelSpecialniType a drobna uprava kodu
		// do tridy Uloha pridat novou polozku do listu TypKombinace
		// definovat samotnou tridu
		// ve tride Sudoku pridat novou polozku do funkce NactiResitele

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sudoku {
	class Program {
		static void Main() {
			var rozmer = 9;
			var sudokuName = "sudoku_1.csv";
			var sudokuPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
			var uloha = new UlohaSudoku(sudokuPath, sudokuName, rozmer, rozmer);
			var sudoku = new Sudoku(uloha);
			sudoku.VyresSudoku();
			Console.Read();
		}
	}
}
