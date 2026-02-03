# language: fr
Fonctionnalité: Calcul de la prime d'assurance (Tarification)
  En tant que système de facturation
  Je veux calculer le prix annuel
  Afin de proposer un devis au client

  # Contexte global des prix de base :
  # - Voiture : 500€
  # - Moto : 400€
  # - Trottinette/Vélo : 100€

  Plan du Scénario: Calcul du prix de base selon le véhicule
    Etant donné une souscription pour un véhicule de type "<type_vehicule>"
    Quand je calcule le devis
    Alors le prix de base est de <prix_attendu> €

    Exemples:
      | type_vehicule          | prix_attendu |
      | Voiture                | 500          |
      | Moto                   | 400          |
      | Trottinette électrique | 100          |
      | Vélo électrique        | 100          |

  # Règle : Les véhicules électriques bénéficient d'une réduction "Verte" de 20%
  Scénario: Application du bonus écologique pour voiture électrique
    Etant donné une "Voiture" à motorisation "Electrique"
    Et le prix de base est de 500 €
    Quand je finalise la tarification
    Alors une réduction de 20% est appliquée
    Et le prix final est de 400 €

  # Règle : Surcoût conducteur novice (+50% si permis < 3 ans)
  Scénario: Surcoût jeune conducteur sur une moto thermique
    Etant donné une "Moto" à motorisation "Essence"
    Et le conducteur a 1 an de permis
    Et le prix de base est de 400 €
    Quand je finalise la tarification
    Alors une majoration de 50% est appliquée
    Et le prix final est de 600 €
