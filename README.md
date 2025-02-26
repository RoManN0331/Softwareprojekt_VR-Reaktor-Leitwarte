# Softwareprojekt - VR Reaktor Leitwarte

Das Softwareprojekt `VR Reaktor Leitwarte` ist eine im Rahmen des Softwarepraktikums entstandene Reaktorsimulation, 
welche in der `VR` Umgebung umgesetzt wurde und unterschiedliche `Gaze Guiding` Elemente zur Unterstützung der Steuerung
implementiert. Das Projekt wurde im Wintersemester 2024/25 an der Universität Trier entwickelt und bindet zusätzlich das
Nebenprojekt [Nuclear Power Plant Simulator - REST API](https://github.com/RoManN0331/Nuclear-Power-Plant-Simulator-REST-API) ein.

## Inhalt

- [Projektbeschreibung](#projektbeschreibung)
- [Dependencies](#dependencies)
- [Mitwirkende](#mitwirkende)


## Projektbeschreibung

Das Projekt setzt sich aus folgenden Komponenten zusammen:

### `NPP Client/Server`
Server und Client, welche die zentrale Aufgabe der Datenbereitstellung und -verarbeitung bezüglich 
der Reaktorsimulation übernehmen. Der Server stellt die Daten der Reaktorsimulation über eine REST API bereit. Ein 
entsprechender Client stellt alle notwendigen Funktionen zur Verfügung, welche der Steuerung bzw. Überwachung des Reaktors
dienen und über die _Simulationsumgebung_ zugänglich sind. Siehe [Nuclear Power Plant Simulator - REST API](https://github.com/RoManN0331/Nuclear-Power-Plant-Simulator-REST-API) für
eine detaillierte Beschreibung der REST API.
<hr>

### `Animator`
Das Zustandsmodell des Reaktors wird durch den Unity Animator modelliert. Der Animator ermöglicht das Erfassen
von Zuständen und Zustandsübergängen. Hieraus kann eine klare Struktur der erlaubten Zustände abgeleitet werden welche 
zusätzlich durch Verknüpfen von `Behaviour`-Scripts für die Steuerung der Gaze Guiding Elemente genutzt werden.
<hr>

### `Gaze Guiding`
Dieses Projekt implementiert verschiedene Gaze Guiding Elemente sowie Hilfsmittel, welche dem Nutzer bei der Bedienung des
Reaktors unterstützen sollen. Gaze Guiding kann auf unterschiedliche Weise eingesetzt werden, um eine Reaktion des Nutzers
herbeizuführen und dabei helfen, in komplexen Situationen einen Überblick zu behalten. Dabei werden unterschiedliche 
Wirkungsmechanismen eingesetzt, um die Aufmerksamkeit des Nutzers zu lenken. Die folgende Tabelle liefert einen Überblick
der implementierten Gaze Guiding Elemente.

| Bezeichnung                                 | Art                                        | Beschreibung                                                                                                                                                                                           |
|---------------------------------------------|--------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Rotationspfeile` (Kontinuierlich, Diskret) | <code style="color : aqua">HM</code>       | Typischerweise an Drehreglern positioniert. Signalisiert, dass eine Aktion an dem entsprechenden Regler nötig ist und gibt die Drehrichtung an. Nur sichtbar in unmittelbarer Nähe.                    |
| `3D Pfeil`                                  | <code style="color : darkorange">GG</code> | Dreidimensionaler Pfeil, befindet sich zu jeder Zeit im Sichtfeld des Nutzers und deutet in die Richtung der auszuführenden Aktion.                                                                    |
| `Path Pfeile`                               | <code style="color : darkorange">GG</code> | Ein oder mehrere Pfeile, welche einen Weg (Luftlinie) zu ausführbaren Aktionen abbilden.                                                                                                               |
| `Anzeige Indikatoren`                       | <code style="color : aqua">HM</code>       | An Anzeigen angebrachten visuelle Indikatoren, bestehend aus einem roten Ausrufezeichen (Signalisiert eine erforderliche Aktion) und grünem Pfeil (Verdeutlicht Ausführung der erforderlichen Aktion). |
| `Direction Cues`                            | <code style="color : darkorange">GG</code> | Rote Randerscheinungen welche aktiv werden, sobald die Blickrichtung von der eigentlichen position der erforderlichen Aktion abweicht.                                                                 |
| `Head Up Display`                           | <code style="color : darkorange">GG</code> | Bildet aktuelle Aufgabe als Text in Form eines HUDs oberhalb der Blickrichtung ab.                                                                                                                     |
| `Clipboard`                                 | <code style="color : aqua">HM</code>       | Heben aktuelle Aufgabe in einem auffälligen Farbton von den restlichen Schritten des Clipboards hervor.                                                                                                |
| `Objekt Detach`                             | <code style="color : aqua">HM</code>       | Blendet für die auszuführende Aktion irrelevante Anzeigen und Regler vollständig aus.                                                                                                                  |
| `Direktionaler Blur`                        | <code style="color : aqua">HM</code>       | Unschärfe Filter auf ferne Objekte.                                                                                                                                                                    |
| `Objekt Highlights`                         | <code style="color : darkorange">GG</code> | Relevante Objekte werden durch rote Rahmen und helles Leuchten hervorgehoben.                                                                                                                          |


- <code style="color : aqua">HM</code> - Hilfsmittel
- <code style="color : darkorange">GG</code> - Gaze Guiding


Gaze Guiding Elemente können anhand eines `Hilfsmittel & Gaze Guiding Panels` aktiviert, bzw. deaktiviert werden.
Zusätzlich sind [Bilder](Docs/ggimages.md) zu den oben genannten Gaze Guiding Elementen zu finden.

<hr>

### `Simulationsumgebung`
Die Simulationsumgebung ist die interagierbare Schnittstelle zur Reaktorsimulation. Hierbei handelt
es sich um einen virtuellen Raum einer Reaktorleitwarte, welcher eine zentrale Mittelkonsole mit verschiedenen Reglern
und Anzeigen zur Steuerung des Reaktors bietet. Zusätzlich stehen Klemmbretter mit Anweisungen für drei Szenarien
(_Hochfahren_, _Herunterfahren_, _Notfallabschaltung_), sowie ein _Hilfsmittel & Gaze Guiding Panel_ zur Verfügung.


## Dependencies

- [Open XR](https://www.khronos.org/openxr/): Offener Standard für VR- und AR Anwendungen. Verwendet für VR Integration
- [Json.NET](https://www.newtonsoft.com/json): Framework zur De-/Serialisierung und von JSON-Daten


## Mitwirkende
Teilnehmer des Softwarepraktikums:

- [Christian Kehl](https://github.com/Chrizzly02)
- [Robert Mersiowsky](https://github.com/rmers)
- [Roman Schander](https://github.com/RoManN0331)
- [Maxim Smirnov](https://github.com/masmir123)