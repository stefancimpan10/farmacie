Operatii care vor fi in aplicatie : 
- **[1] Vizualizare Catalog**
- **[2] Adăugare Produs Nou**
- **[3] Actualizare Stoc:**
- **[4] Verificare Produse Expirate**
- **[5] Creare Comandă Nouă**
- **[6] Căutare Rapidă** # Cautare dupa nume sau substanta
- **[7] Vizualizare Istoric Vânzări**
- **[8] Stergere/Dezactivare produs**
- **[9] Modificare pret**
- **X Iesire**
  

Se vor stoca informatiile pentru fiecare medicament : 
- **ID Produs**
- **Denumire comercială**
- **Substanță activă**
- **Formă farmaceutică**
- **Concentrație**
- **Preț unitar**
- **Stoc disponibil**
- **Necesită Rețetă**
- **Data Expirării**

Se vor stoca informatiile pentru fiecare comanda : 
- **ID Produs**
- **Data si ora**
- **Lista articole :**
        - ID Produs
        - Cantitatea cumparata 
        - Pret la momentul vanzarii
- **Date client :**
        - Nume si prenume
        - Numar de telefon
- **Daca necesita reteta :**
        - Cod parafa medic
        - Serie/Numar reteta
- **Total plata**
- **Metoda de Plata**
- **Status plata**


Clasele necesare implementării aplicației:

- **Clasa Produs:** Reprezintă medicamentul. Conține toate detaliile tehnice, prețul și informațiile de siguranță (rețetă, expirare).

- **Clasa ArticolComanda:** Este elementul de legătură. Conține un obiect de tip Produs și cantitatea dorită, calculând automat prețul pentru acea linie din comandă.

- **Clasa Comanda:** Reprezintă tranzacția finală. Stochează datele clientului, lista de articole, totalul de plată și detaliile rețetei dacă produsul a fost marcat ca fiind restrictiv.

- **Clasa Manager:** Este centrul de comandă al aplicației. Gestionează listele globale (stocul și istoricul) și execută toate operațiile de adăugare, ștergere sau căutare.

    Ar arata ceva :

**Clasa Produs:** Constructor, GetID, SetID, GetDenumire, SetDenumire, GetSubstanta, SetSubstanta, GetForma, SetForma, GetConcentratie, SetConcentratie, GetPret, SetPret, GetStoc, SetStoc, GetReteta, SetReteta, GetDataExpirarii, SetDataExpirarii.

**Clasa ArticolComanda:** Constructor, GetProdus, SetProdus, GetCantitate, SetCantitate, GetPretTotalArticol.

**Clasa Comanda:** Constructor, GetID, SetID, GetNumeClient, SetNumeClient, GetProduse, SetProduse, GetPretTotal, GetDataOra, SetDataOra, GetStatusPlata, SetStatusPlata, GetCodParafa, SetCodParafa, AdaugaArticol.

**Clasa Manager:** AdaugaProdus, StergeProdus, ModificaPret, ActualizeazaStoc, ObtineMeniu, CautaProdus, CautaRapid, VerificaExpirate, AdaugaComanda, ObtineComenzi, CautaComanda, ModificaComanda, StergeComanda.


🏥 PharmaSuite Pro - Sistem de Gestiune Farmaceutică
📌 Descriere Generală
PharmaSuite Pro este o aplicație desktop modernă dezvoltată în C# .NET (WPF), destinată gestionării complete a unei farmacii. Aplicația permite administrarea stocurilor, procesarea vânzărilor, monitorizarea expirărilor și generarea de rapoarte financiare, utilizând o arhitectură modulară și o bază de date locală portabilă (JSON).
🚀 Funcționalități Principale
Aplicația acoperă toate operațiile esențiale ale unui flux farmaceutic:
📋 Vizualizare Catalog & Inventar: Listă completă a medicamentelor cu detalii tehnice (substanță, formă, concentrație) și valori de stoc.
🛒 Creare Comandă Nouă (Vânzare): Interfață tip "POS" rapidă, cu coș de cumpărături, calcul automat al totalului și validare stoc în timp real.
📦 Recepție Marfă: Modul dedicat pentru intrări de stoc, permitând adăugarea multiplă de produse (tip factură) și actualizarea automată a cantităților.
⚠️ Alerte Expirare: Monitorizare proactivă a produselor apropiate de data expirării, cu filtre dinamice (30 zile, 3 luni, 1 an).
💰 Rapoarte Financiare: Dashboard detaliat cu încasări zilnice/lunare, număr de comenzi și istoric tranzacții filtrabil pe perioade.
⚙️ Administrare & Configurare: Editare prețuri, activare/dezactivare produse și gestionarea parametrilor sistemului.
💱 Conversie Valutară: Instrument utilitar pentru conversia rapidă a sumelor între RON, EUR și USD.
🏗️ Arhitectură Software & Clase
Aplicația este construită pe principiul separării responsabilităților, folosind următoarele clase fundamentale:
1. Clasa MedicamentDB (Produs)
Reprezintă entitatea de bază a catalogului. Stochează informațiile tehnice și comerciale ale medicamentului.
Atribute: ID, Nume (Denumire comercială), SubstantaActivă, FormaFarmaceutică, Concentrație, Pret, Stoc, NecesitaReteta, DataExpirare.
Responsabilități: Definirea structurii datelor pentru un produs individual.
2. Clasa ArticolComanda (Linie de Vânzare)
Elementul de legătură dintre un produs și o tranzacție specifică.
Atribute: Produs (referință către MedicamentDB), Cantitate, PretVanzareMomentan.
Responsabilități: Calculează subtotalul pentru o linie din coș (Cantitate * Pret) și îngheață prețul la momentul vânzării, indiferent de modificările ulterioare din catalog.
3. Clasa Comanda (Tranzacție)
Reprezintă o vânzare finalizată.
Atribute: ID, DataOra, ListaArticole (colecție de ArticolComanda), TotalPlata, DateClient (Nume, Telefon), DetaliiReteta (Cod Parafă, Serie/Nr.), MetodaPlata.
Responsabilități: Agregarea articolelor, calculul totalului final și stocarea datelor clientului pentru conformitate legală.
4. Clasa ManagerMedicamente (Controller/Service)
Centrul de comandă al aplicației. Gestionează persistența datelor și logica de business.
Metode Cheie:
IncarcaMedicamente(): Citește baza de date JSON.
SalveazaMedicamente(List<MedicamentDB>): Scrie modificările în JSON.
ActualizeazaStoc(int id, int cantitate): Scade stocul după o vânzare.
CautaProdus(string keyword): Filtrează lista după nume sau substanță activă.
VerificaExpirate(int zile): Returnează lista produselor critice.
💾 Persistența Datelor
Format: Fișiere JSON (medicamente.json, comenzi_farmacie.json).
Librărie: Newtonsoft.Json pentru serializare/deserializare rapidă și robustă.
Avantaj: Portabilitate maximă (nu necesită instalare SQL Server), ușor de backup-uit și editat manual dacă este nevoie.
🎨 Interfața Utilizator (WPF)
Interfața este dezvoltată în XAML, oferind o experiență fluidă și modernă:
DataGrid-uri personalizate: Pentru vizualizarea și editarea tabelelor complexe.
Validare Input: Controale care previn introducerea de date eronate (ex: cantități negative, prețuri invalide, date expirate).
Navigare Dinamică: Schimbarea paginilor fără reîncărcarea ferestrei principale, asigurând o curgere naturală a fluxului de lucru.
🛠️ Tehnologii Utilizate
Limbaj: C# 12 / .NET 10
Framework UI: Windows Presentation Foundation (WPF)
Serializare: Newtonsoft.Json
IDE: Visual Studio 2026