# language: fr
Fonctionnalité: Gestion du risque et de la Franchise
  En tant qu'expert risque
  Je veux définir le montant de la franchise en cas d'accident
  Afin de limiter les pertes de l'assureur

  # La franchise est la part payée par le client en cas de sinistre.
  # Elle dépend de la valeur du véhicule et du profil conducteur.

  Scénario: Franchise standard pour un bon conducteur
    Etant donné un conducteur avec un bonus de 0.50 (50% de bonus)
    Et une voiture d'une valeur de 20000 €
    Quand je calcule la franchise accident
    Alors la franchise est de 500 € (fixe)

  Scénario: Franchise punitive pour conducteur à risque (Malussé)
    Etant donné un conducteur avec un malus de 1.25 (25% de malus)
    Et une voiture d'une valeur de 20000 €
    Quand je calcule la franchise accident
    Alors la franchise est calculée à 10% de la valeur du véhicule
    Et le montant de la franchise est de 2000 €

  Scénario: Franchise réduite pour mobilité douce (Vélo/Trottinette)
    Etant donné un véhicule de type "Vélo électrique"
    Et une valeur d'achat de 2000 €
    Quand je calcule la franchise accident
    Alors la franchise est plafonnée à 50 €
