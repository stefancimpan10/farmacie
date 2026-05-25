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


# PharmaSuite Pro - Sistem de Gestiune Farmaceutică

## **📌 Descriere Generală**
**PharmaSuite Pro** este o aplicație desktop modernă dezvoltată în **C# .NET (WPF)**, destinată gestionării complete a unei farmacii.

Aplicația permite:
- administrarea stocurilor
- procesarea vânzărilor
- monitorizarea expirărilor
- generarea de rapoarte financiare

folosind o arhitectură modulară și o bază de date locală portabilă bazată pe fișiere JSON.

---

# **🚀 Funcționalități Principale**

Aplicația acoperă toate operațiile esențiale ale unui flux farmaceutic.

## **📋 Vizualizare Catalog & Inventar**
- Listă completă a medicamentelor
- Detalii tehnice:
  - substanță activă
  - formă farmaceutică
  - concentrație
- Vizualizare stoc disponibil
- Filtrare și căutare rapidă

---

## **🛒 Creare Comandă Nouă (Vânzare)**
- Interfață tip POS rapidă
- Coș de cumpărături
- Calcul automat al totalului
- Validare stoc în timp real
- Verificare produse care necesită rețetă

---

## **📦 Recepție Marfă**
- Modul dedicat pentru intrări de stoc
- Adăugare multiplă de produse
- Actualizare automată a cantităților
- Gestionare tip factură

---

## **⚠️ Alerte Expirare**
- Monitorizare produse apropiate de expirare
- Filtre dinamice:
  - 30 zile
  - 3 luni
  - 1 an

---

## **💰 Rapoarte Financiare**
- Încasări zilnice
- Încasări lunare
- Număr total de comenzi
- Istoric tranzacții filtrabil
- Dashboard statistic

---

## **⚙️ Administrare & Configurare**
- Editare prețuri
- Activare/dezactivare produse
- Gestionare parametri sistem

---

## **💱 Conversie Valutară**
- Conversie rapidă:
  - RON
  - EUR
  - USD

---

# **🧩 Operații Disponibile în Aplicație**

- **[1] Vizualizare Catalog**
- **[2] Adăugare Produs Nou**
- **[3] Actualizare Stoc**
- **[4] Verificare Produse Expirate**
- **[5] Creare Comandă Nouă**
- **[6] Căutare Rapidă**
  - căutare după nume
  - căutare după substanță activă
- **[7] Vizualizare Istoric Vânzări**
- **[8] Ștergere / Dezactivare Produs**
- **[9] Modificare Preț**
- **[X] Ieșire**

---

# **💾 Structura Datelor**

## **📦 Informații Stocate pentru Fiecare Medicament**

- **ID Produs**
- **Denumire Comercială**
- **Substanță Activă**
- **Formă Farmaceutică**
- **Concentrație**
- **Preț Unitar**
- **Stoc Disponibil**
- **Necesită Rețetă**
- **Data Expirării**

---

## **🧾 Informații Stocate pentru Fiecare Comandă**

- **ID Comandă**
- **Data și Ora**
- **Lista Articole**
  - ID Produs
  - Cantitatea cumpărată
  - Preț la momentul vânzării
- **Date Client**
  - Nume și prenume
  - Număr de telefon
- **Dacă necesită rețetă**
  - Cod parafă medic
  - Serie / Număr rețetă
- **Total Plată**
- **Metoda de Plată**
- **Status Plată**

---

# **🏗️ Arhitectură Software & Clase**

Aplicația este construită pe principiul separării responsabilităților.

---

## **1. Clasa MedicamentDB (Produs)**

Reprezintă entitatea de bază a catalogului.

### **Atribute**
- ID
- Nume
- SubstantaActivă
- FormaFarmaceutică
- Concentrație
- Pret
- Stoc
- NecesitaReteta
- DataExpirare

### **Responsabilități**
- definirea structurii datelor pentru un produs individual

---

## **2. Clasa ArticolComanda (Linie de Vânzare)**

Elementul de legătură dintre un produs și o tranzacție.

### **Atribute**
- Produs
- Cantitate
- PretVanzareMomentan

### **Responsabilități**
- calculează subtotalul unei linii din comandă
- păstrează prețul produsului la momentul vânzării

---

## **3. Clasa Comanda (Tranzacție)**

Reprezintă o vânzare finalizată.

### **Atribute**
- ID
- DataOra
- ListaArticole
- TotalPlata
- DateClient
- DetaliiReteta
- MetodaPlata

### **Responsabilități**
- agregarea articolelor
- calcularea totalului
- stocarea informațiilor clientului

---

## **4. Clasa ManagerMedicamente (Controller / Service)**

Centrul de comandă al aplicației.

### **Metode Principale**
- IncarcaMedicamente()
- SalveazaMedicamente()
- ActualizeazaStoc()
- CautaProdus()
- VerificaExpirate()

---

# **🧠 Clase și Metode**

## **Clasa Produs**
- Constructor
- GetID()
- SetID()
- GetDenumire()
- SetDenumire()
- GetSubstanta()
- SetSubstanta()
- GetForma()
- SetForma()
- GetConcentratie()
- SetConcentratie()
- GetPret()
- SetPret()
- GetStoc()
- SetStoc()
- GetReteta()
- SetReteta()
- GetDataExpirarii()
- SetDataExpirarii()

---

## **Clasa ArticolComanda**
- Constructor
- GetProdus()
- SetProdus()
- GetCantitate()
- SetCantitate()
- GetPretTotalArticol()

---

## **Clasa Comanda**
- Constructor
- GetID()
- SetID()
- GetNumeClient()
- SetNumeClient()
- GetProduse()
- SetProduse()
- GetPretTotal()
- GetDataOra()
- SetDataOra()
- GetStatusPlata()
- SetStatusPlata()
- GetCodParafa()
- SetCodParafa()
- AdaugaArticol()

---

## **Clasa Manager**
- AdaugaProdus()
- StergeProdus()
- ModificaPret()
- ActualizeazaStoc()
- ObtineMeniu()
- CautaProdus()
- CautaRapid()
- VerificaExpirate()
- AdaugaComanda()
- ObtineComenzi()
- CautaComanda()
- ModificaComanda()
- StergeComanda()

---

# **💾 Persistența Datelor**

## **Format**
- `medicamente.json`
- `comenzi_farmacie.json`

## **Librărie**
- `Newtonsoft.Json`

## **Avantaje**
- portabilitate ridicată
- fără SQL Server
- backup rapid
- editare manuală ușoară

---

# **🎨 Interfața Utilizator (WPF)**

- DataGrid-uri personalizate
- Validare input
- Navigare dinamică
- Interfață modernă și fluidă

---

# **🛠️ Tehnologii Utilizate**

- **Limbaj:** C# 12 / .NET 10
- **Framework UI:** WPF
- **Serializare:** Newtonsoft.Json
- **IDE:** Visual Studio 2026
