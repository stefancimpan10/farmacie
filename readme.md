Operatii care vor fi in aplicatie : 
- [1] Vizualizare Catalog
- [2] Adăugare Produs Nou
- [3] Actualizare Stoc:
- [4] Verificare Produse Expirate
- [5] Creare Comandă Nouă
- [6] Căutare Rapidă # Cautare dupa nume sau substanta
- [7] Vizualizare Istoric Vânzări
- [8] Stergere/Dezactivare produs
- [9] Modificare pret
- X Iesire
  [x]

Se vor stoca informatiile pentru fiecare medicament : 
- ID Produs
- Denumire comercială
- Substanță activă
- Formă farmaceutică
- Concentrație
- Preț unitar
- Stoc disponibil
- Necesită Rețetă
- Data Expirării

Se vor stoca informatiile pentru fiecare comanda : 
- ID Produs
- Data si ora
- Lista articole :
        - ID Produs
        - Cantitatea cumparata 
        - Pret la momentul vanzarii
- Date client :
        - Nume si prenume
        - Numar de telefon
- Daca necesita reteta :
        - Cod parafa medic
        - Serie/Numar reteta
- Total plata
- Metoda de Plata
- Status plata


Clasele necesare implementării aplicației:

-  Clasa Produs: Reprezintă medicamentul. Conține toate detaliile tehnice, prețul și informațiile de siguranță (rețetă, expirare).

- Clasa ArticolComanda: Este elementul de legătură. Conține un obiect de tip Produs și cantitatea dorită, calculând automat prețul pentru acea linie din comandă.

- Clasa Comanda: Reprezintă tranzacția finală. Stochează datele clientului, lista de articole, totalul de plată și detaliile rețetei dacă produsul a fost marcat ca fiind restrictiv.

- Clasa Manager: Este centrul de comandă al aplicației. Gestionează listele globale (stocul și istoricul) și execută toate operațiile de adăugare, ștergere sau căutare.

    Ar arata ceva :

Clasa Produs: Constructor, GetID, SetID, GetDenumire, SetDenumire, GetSubstanta, SetSubstanta, GetForma, SetForma, GetConcentratie, SetConcentratie, GetPret, SetPret, GetStoc, SetStoc, GetReteta, SetReteta, GetDataExpirarii, SetDataExpirarii.

Clasa ArticolComanda: Constructor, GetProdus, SetProdus, GetCantitate, SetCantitate, GetPretTotalArticol.

Clasa Comanda: Constructor, GetID, SetID, GetNumeClient, SetNumeClient, GetProduse, SetProduse, GetPretTotal, GetDataOra, SetDataOra, GetStatusPlata, SetStatusPlata, GetCodParafa, SetCodParafa, AdaugaArticol.

Clasa Manager: AdaugaProdus, StergeProdus, ModificaPret, ActualizeazaStoc, ObtineMeniu, CautaProdus, CautaRapid, VerificaExpirate, AdaugaComanda, ObtineComenzi, CautaComanda, ModificaComanda, StergeComanda.
