# Graph Report - .  (2026-07-04)

## Corpus Check
- Large corpus: 149 files · ~1,892,613 words. Semantic extraction will be expensive (many Claude tokens). Consider running on a subfolder.

## Summary
- 253 nodes · 302 edges · 21 communities
- Extraction: 98% EXTRACTED · 2% INFERRED · 0% AMBIGUOUS · INFERRED: 6 edges (avg confidence: 0.83)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Core Timer Engine|Core Timer Engine]]
- [[_COMMUNITY_Home Page & Settings|Home Page & Settings]]
- [[_COMMUNITY_App Lifecycle|App Lifecycle]]
- [[_COMMUNITY_About & Donate Pages|About & Donate Pages]]
- [[_COMMUNITY_Animation Items|Animation Items]]
- [[_COMMUNITY_Async Helpers|Async Helpers]]
- [[_COMMUNITY_Settings Page UI|Settings Page UI]]
- [[_COMMUNITY_Documentation & Build Artifacts|Documentation & Build Artifacts]]
- [[_COMMUNITY_Statistics Logging|Statistics Logging]]
- [[_COMMUNITY_Main Page Navigation|Main Page Navigation]]
- [[_COMMUNITY_Settings Storage|Settings Storage]]
- [[_COMMUNITY_Encryption Module|Encryption Module]]
- [[_COMMUNITY_Encryptor CLI|Encryptor CLI]]
- [[_COMMUNITY_Animation Management|Animation Management]]
- [[_COMMUNITY_NuGet Dependencies|NuGet Dependencies]]
- [[_COMMUNITY_Core Utilities|Core Utilities]]

## God Nodes (most connected - your core abstractions)
1. `Core` - 27 edges
2. `Home` - 22 edges
3. `AnimationsItem` - 16 edges
4. `App` - 14 edges
5. `SettingsPage` - 12 edges
6. `ExclusiveSynchronizationContext` - 10 edges
7. `ConcentrateOnSettings` - 10 edges
8. `MainPage` - 9 edges
9. `StatisticsLog` - 7 edges
10. `Statistics` - 6 edges

## Surprising Connections (you probably didn't know these)
- `ConcentrateOn Application` --references--> `ConcentrateOn MSIX Package v1.0.4.0`  [INFERRED]
  About.txt → ConcentrateOn/BundleArtifacts/Upload/arm.txt
- `Encryptor C# Project` --conceptually_related_to--> `ConcentrateOn Application`  [INFERRED]
  Encryptor/obj/Debug/Encryptor.csproj.FileListAbsolute.txt → About.txt
- `ARM Upload Build Manifest v1.0.4.0` --semantically_similar_to--> `ARM Build Artifacts Manifest v1.0.4.0`  [INFERRED] [semantically similar]
  ConcentrateOn/BundleArtifacts/Upload/arm.txt → ConcentrateOn/BundleArtifacts/arm.txt
- `x64 Upload Build Manifest v1.0.4.0` --semantically_similar_to--> `x64 Build Artifacts Manifest v1.0.4.0`  [INFERRED] [semantically similar]
  ConcentrateOn/BundleArtifacts/Upload/x64.txt → ConcentrateOn/BundleArtifacts/x64.txt
- `x86 Upload Build Manifest v1.0.4.0` --semantically_similar_to--> `x86 Build Artifacts Manifest v1.0.4.0`  [INFERRED] [semantically similar]
  ConcentrateOn/BundleArtifacts/Upload/x86.txt → ConcentrateOn/BundleArtifacts/x86.txt

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Multi-Architecture Build Manifests (ARM, x64, x86)** — upload_arm, upload_x64, upload_x86, bundleartifacts_arm, bundleartifacts_x64, bundleartifacts_x86 [INFERRED 0.85]
- **ConcentrateOn Solution Components** — about_concentrateonapp, encryptor_project, concentrateon_msix_v1040 [INFERRED 0.75]

## Communities (21 total, 0 thin omitted)

### Community 0 - "Core Timer Engine"
Cohesion: 0.10
Nodes (11): Animations, AnimationsItem, Core, ConcentrateOnSettings, DateTimeOffset, Home, int, string (+3 more)

### Community 1 - "Home Page & Settings"
Cohesion: 0.10
Nodes (13): ApplicationDataContainer, ConcentrateOn, App, bool, ConcentrateOnSettings, Core, double, NavigatingCancelEventArgs (+5 more)

### Community 2 - "App Lifecycle"
Cohesion: 0.11
Nodes (12): Application, App, ConcentrateOn, Core, Home, EnteredBackgroundEventArgs, IActivatedEventArgs, LaunchActivatedEventArgs (+4 more)

### Community 3 - "About & Donate Pages"
Cohesion: 0.11
Nodes (12): About, ConcentrateOn, NavigationEventArgs, ConcentrateOn, NavigationEventArgs, DonatePage, ConcentrateOn, App (+4 more)

### Community 4 - "Animation Items"
Cohesion: 0.16
Nodes (7): AnimationsItem, ConcentrateOn, bool, double, string, CurrentMediaPlaybackItemChangedEventArgs, MediaPlaybackList

### Community 5 - "Async Helpers"
Cohesion: 0.15
Nodes (11): AutoResetEvent, AsyncHelpers, ConcentrateOn, bool, T, ExclusiveSynchronizationContext, Func, Queue (+3 more)

### Community 6 - "Settings Page UI"
Cohesion: 0.13
Nodes (10): ConcentrateOn, App, ConcentrateOnSettings, Core, NavigatingCancelEventArgs, NavigationEventArgs, RoutedEventArgs, SelectionChangedEventArgs (+2 more)

### Community 7 - "Documentation & Build Artifacts"
Cohesion: 0.15
Nodes (17): About.txt - Application Description, ConcentrateOn Application, Francesco Cirillo, Pomodoro Technique, Timer-Based Interval Work and Rest Cycle, ARM Build Artifacts Manifest v1.0.4.0, x64 Build Artifacts Manifest v1.0.4.0, x86 Build Artifacts Manifest v1.0.4.0 (+9 more)

### Community 8 - "Statistics Logging"
Cohesion: 0.16
Nodes (10): ActivityType, ConcentrateOn, DateTimeOffset, int, long, string, StatisticsLog, StatisticsLogItem (+2 more)

### Community 9 - "Main Page Navigation"
Cohesion: 0.15
Nodes (7): ConcentrateOn, App, NavigationEventArgs, RoutedEventArgs, SelectionChangedEventArgs, MainPage, PointerRoutedEventArgs

### Community 10 - "Settings Storage"
Cohesion: 0.21
Nodes (7): ConcentrateOn, ConcentrateOnSettings, bool, DateTimeOffset, int, long, string

### Community 11 - "Encryption Module"
Cohesion: 0.25
Nodes (4): DllImport, Encryption, Encryptor, IntPtr

### Community 12 - "Encryptor CLI"
Cohesion: 0.31
Nodes (4): string, Encrypt_item, Encryptor, Program

### Community 13 - "Animation Management"
Cohesion: 0.40
Nodes (3): Animations, ConcentrateOn, Dictionary

### Community 14 - "NuGet Dependencies"
Cohesion: 0.40
Nodes (3): Microsoft.NETCore.UniversalWindowsPlatform, Microsoft.Toolkit.Uwp.Notifications, Microsoft.Toolkit.Uwp.UI.Controls

### Community 15 - "Core Utilities"
Cohesion: 0.40
Nodes (3): ConcentrateOn, T, Helper

## Knowledge Gaps
- **88 isolated node(s):** `ConcentrateOn`, `NavigationEventArgs`, `ConcentrateOn`, `Dictionary`, `ConcentrateOn` (+83 more)
  These have ≤1 connection - possible missing edges or undocumented components.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Home` connect `Home Page & Settings` to `About & Donate Pages`?**
  _High betweenness centrality (0.055) - this node is a cross-community bridge._
- **Why does `SettingsPage` connect `Settings Page UI` to `About & Donate Pages`?**
  _High betweenness centrality (0.038) - this node is a cross-community bridge._
- **Why does `MainPage` connect `Main Page Navigation` to `About & Donate Pages`?**
  _High betweenness centrality (0.030) - this node is a cross-community bridge._
- **What connects `ConcentrateOn`, `NavigationEventArgs`, `ConcentrateOn` to the rest of the system?**
  _89 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Core Timer Engine` be split into smaller, more focused modules?**
  _Cohesion score 0.10317460317460317 - nodes in this community are weakly interconnected._
- **Should `Home Page & Settings` be split into smaller, more focused modules?**
  _Cohesion score 0.09788359788359788 - nodes in this community are weakly interconnected._
- **Should `App Lifecycle` be split into smaller, more focused modules?**
  _Cohesion score 0.11255411255411256 - nodes in this community are weakly interconnected._