# language: fr
Fonctionnalité: Gestion des sinistres du conducteur
  En tant qu'assureur
  Je veux gérer les sinistres associés à un conducteur
  Afin de calculer les surcharges et évaluer le risque

  # Règle : Un sinistre peut être enregistré pour un conducteur
  Scénario: Ajout d'un sinistre à un conducteur
    Etant donné un conducteur nommé "Jean Dupont"
    Et le conducteur n'a pas de sinistre
    Quand j'ajoute un sinistre d'un montant de 5000 €
    Alors le conducteur a maintenant 1 sinistre
    Et le montant total des sinistres est de 5000 €

  # Règle : Plusieurs sinistres s'accumulent
  Scénario: Accumulation de plusieurs sinistres
    Etant donné un conducteur nommé "Marie Martin"
    Et le conducteur a 2 sinistres précédents pour 3000 € et 2000 €
    Quand j'ajoute un sinistre d'un montant de 4000 €
    Alors le conducteur a maintenant 3 sinistres
    Et le montant total des sinistres est de 9000 €

  # Règle : Un sinistre récent (< 1 an) applique une surcharge
  Scénario: Surcharge pour sinistre récent
    Etant donné un conducteur nommé "Pierre Lefevre"
    Et le conducteur a un sinistre depuis 200 jours
    Quand je vérifie les sinistres récents sur 365 jours
    Alors le conducteur a un sinistre récent
    Et une surcharge de 25% est appliquée

  # Règle : Un sinistre ancien (> 1 an) n'applique pas de surcharge
  Scénario: Pas de surcharge pour sinistre ancien
    Etant donné un conducteur nommé "Sophie Bernard"
    Et le conducteur a un sinistre depuis 500 jours
    Quand je vérifie les sinistres récents sur 365 jours
    Alors le conducteur n'a pas de sinistre récent
    Et aucune surcharge n'est appliquée

  # Règle : Le coefficient de surprime augmente avec le nombre de sinistres
  Scénario: Coefficient de surprime selon le nombre de sinistres
    Etant donné un conducteur nommé "Luc Rousseau"
    Et le conducteur a 4 sinistres
    Quand je calcule le coefficient de surprime
    Alors le coefficient est de 1.50

  # Règle : Les sinistres après un certain délai peuvent être supprimés
  Scénario: Suppression des sinistres de plus de 2 ans
    Etant donné un conducteur nommé "Anne Dubois"
    Et le conducteur a 3 sinistres datant de 800, 1000 et 1200 jours
    Quand je supprime les sinistres de plus de 730 jours (2 ans)
    Alors le conducteur a maintenant 2 sinistres
    Et les sinistres restants datent de 800 et 1000 jours
