# language: fr
Fonctionnalité: Fidélité et Offres Famille
  En tant que responsable commercial
  Je veux récompenser les bons clients et les familles
  Afin d'augmenter la rétention client

  # Règle : Offre multi-véhicules (le 2ème véhicule est moins cher)
  Scénario: Réduction sur le second véhicule assuré
    Etant donné un client assurant déjà 1 véhicule actif
    Quand il assure un 2ème véhicule de type "Voiture"
    Et que le prix normal du nouveau contrat est 500 €
    Alors une remise "Multi-auto" de 15% est appliquée
    Et le prix final du second contrat est 425 €

  # Règle : Fidélité "Or" annule la franchise bris de glace
  Scénario: Client fidèle exonéré de franchise bris de glace
    Etant donné un client ayant 6 ans d'ancienneté (Statut "Gold")
    Et un sinistre de type "Bris de glace"
    Et une franchise contractuelle de 50 €
    Quand je calcule le montant à payer
    Alors la franchise appliquée est de 0 € (Offert)

  # Règle : Réduction cumulonimbus (Famille + Electrique)
  Scénario: Cumul des avantages
    Etant donné un client avec déjà 1 véhicule
    Quand il assure un "Vélo électrique" (Prix base 100€)
    Et qu'il bénéficie du bonus écologique (-20%)
    Et qu'il bénéficie du bonus multi-équipement (-15% sur le reste)
    Alors le prix intermédiaire est 80 € (Eco)
    Et le prix final est 68 € (Multi)
