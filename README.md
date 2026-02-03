# Mon Assurance

Ce projet est une simulation de gestion d'une compagnie d'assurance automobile.
L'objectif est d'implémenter un moteur de règles complet gérant l'éligibilité, la tarification et le calcul des franchises.

## 🚗 Domaine Métier

L'application gère des contrats pour divers types de véhicules :
*   **Voitures** (Thermique / Électrique)
*   **Motos** (Avec gestion de la puissance)
*   **Mobilités douces** : Trottinettes électriques, Vélos électriques.

## 📜 Règles de Gestion

Le système applique les règles métier suivantes pour chaque domaine fonctionnel :

### 1. Éligibilité
*   **Contrôle de l'âge** : Les mineurs ne peuvent pas assurer de véhicules lourds (Voiture, Moto), mais peuvent accéder aux mobilités douces dès 16 ans.
*   **Expérience conductrice** : L'assurance des motos très puissantes (> 100 ch) est réservée aux conducteurs ayant au moins 5 ans de permis.

### 2. Tarification
*   **Prix de base** : Le tarif annuel dépend du type de véhicule (ex: Voiture 500€, Moto 400€, Trottinette 100€).
*   **Bonus Écologique** : Une réduction de 20% est appliquée automatiquement à tous les véhicules électriques.
*   **Surprime Novice** : Une majoration de 50% est appliquée aux jeunes conducteurs ayant moins de 3 ans de permis.

### 3. Gestion du Risque & Franchise
*   **Calcul dynamique** : Le montant de la franchise (reste à charge) varie selon le profil.
*   **Profil Risqué** : Pour les conducteurs malussés, la franchise est calculée au pourcentage (10%) de la valeur du véhicule.
*   **Protection Mobilité** : Pour les vélos et trottinettes, la franchise est plafonnée à 50€.

### 4. Contexte d'Usage
*   **Stationnement** : Une réduction est accordée si le véhicule dort dans un garage fermé (risque de vol réduit).
*   **Petit Rouleur** : Une remise de 10% s'applique si le conducteur déclare parcourir moins de 8000 km/an.
*   **Exclusions** : Les usages professionnels spécifiques (VTC, Livraison) sont automatiquement rejetés.

### 5. Fidélité & Offres Famille
*   **Offre Multi-contrats** : Dés le second véhicule assuré, une réduction de 15% est appliquée sur ce nouveau contrat.
*   **Statut Gold** : Les clients fidèles (> 5 ans d'ancienneté) bénéficient de la gratuité totale de la franchise "Bris de glace".
*   **Cumul des avantages** : Le système gère l'application successive des différentes réductions (ex: Bonus Écologique puis réduction Multi-contrats).
