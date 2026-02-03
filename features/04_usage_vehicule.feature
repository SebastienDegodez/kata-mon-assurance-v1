# language: fr
Fonctionnalité: Context d'usage et Stationnement
  En tant qu'actuaire
  Je veux ajuster le tarif selon l'usage réel du véhicule
  Afin d'aligner le prix sur le risque réel

  # Règle : Le stationnement en garage fermé est plus sûr que la rue
  Scénario: Réduction pour stationnement sécurisé
    Etant donné un contrat standard à 500 €
    Et le véhicule dort dans un "Garage fermé privé"
    Quand je calcule le coefficient géographique
    Alors une réduction de 5% est appliquée
    Et le tarif ajusté est de 475 €

  # Règle : Les "petits rouleurs" paient moins cher (< 8000 km/an)
  Scénario: Avantage petit rouleur
    Etant donné un conducteur déclarant 5000 km par an
    Et un tarif de base de 600 €
    Quand j'applique le facteur kilométrique
    Alors une remise "Petit Rouleur" de 10% est appliquée
    Et le tarif affiché est de 540 €

  # Règle : Usage professionnel interdit sur contrat particulier
  Scénario: Refus d'usage commercial (Livraison, VTC)
    Etant donné une demande d'assurance pour une "Voiture"
    Et l'usage déclaré est "Transport de personnes à titre onéreux"
    Quand je valide le dossier
    Alors le dossier est rejeté avec motif "Usage professionnel non couvert par ce contrat"
