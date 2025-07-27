# Projet ConvertAppProject

# 1. Projet

ConvertAppProject est un projet console en c# .NET version ```8.0```. Il permet la conversion récursive de fichier au format JSON ou XML. L'application permet également d'ffectuer diverses actions sur les fichiers avant de les convertir comme : 
du groupage, du triage et de la recherche.

# 2. Architecture

L'architecture du projet se décompose en plusieurs dossiers : 

__ConvertAppProject__ :

```
ConvertAppProject
├── Contexts             # contient les fichier c# de logique métier de chaque type de conversion (json, xml, ...)
├── FilesConverted       # contient tous les fichiers après conversion
├── FilesToConvert       # contient les fichiers à convertir
├── Model                # contient les models du projet
│   ├── Action           # contient les classes de logique métier pour appliquer les actions
│   ├── FileConversion   # contient la classe de logique métier pour gérer le processus global de conversion
├── Services             # contient tous les services
├── Tools                # contient toutes les méthode réutilisables dans l'application
├── program.cs           # fichier c# de lancement du projet
```

# 3. Installation
Pour pouvoir utilisé l'application, il faut d'abord y installer tous les éléments nécessaire à son bon fonctionnement. Pour ce faire, la première étape est de :

- Télécharger le projet en cliquant sur ```Download ZIP``` dans la section _Code_ en vert.
- Ou importer le projet via git bash par example via la commande ```git clone https://github.com/Yann-cmd/ConvertAppProject.git```

## 3.1 Informations sur les fichiers
Une fois le projet en main il faut le dézipper et l'ouvrir avec votre IDE __Visual Studio 2022__.

Des fichiers templates ont déjà été placer dans le dossier __FilesToConvert__ pour réaliser éventuellement des tests. Vous êtes libre d'importer d'autres fichiers de votre choix. Cependant, ils doivent suivre un format similaire aux fichiers template. A savoir :

- Pour du JSON, l'outil fonctionne dans le cas où un seul objet se trouve dans le fichier ou que la structure soit exactement similaire à celle du fichier template JSON. (un objet contenant un unique attribut de tableau d'objet).

- Pour du XML, l'outil fonctionne avec une structure exactement similaire à celle du fichier template XML.

_Spécificité : l'application des actions est irreversible tant que le fichier n'est pas convertis ou que vous quittiez le programme_

# 4. Get started
Une fois toutes ces étapes prises en compte, vous pouvez lancer le projet.

Dans votre IDE, rendez-vous en haut de votre écran et cliquez sur l'icone d'une flèche verte __Exécuter__.
