# Graph Report - .  (2026-07-05)

## Corpus Check
- 422 files · ~0 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 422 nodes · 529 edges · 31 communities (29 shown, 2 thin omitted)
- Extraction: 99% EXTRACTED · 1% INFERRED · 0% AMBIGUOUS · INFERRED: 6 edges (avg confidence: 0.83)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Web UI Components|Web UI Components]]
- [[_COMMUNITY_Animation Manifest|Animation Manifest]]
- [[_COMMUNITY_Core Timer Engine|Core Timer Engine]]
- [[_COMMUNITY_Web Dependencies|Web Dependencies]]
- [[_COMMUNITY_UWP Home Page|UWP Home Page]]
- [[_COMMUNITY_UWP App Lifecycle|UWP App Lifecycle]]
- [[_COMMUNITY_UWP About & Donate|UWP About & Donate]]
- [[_COMMUNITY_TypeScript Config (App)|TypeScript Config (App)]]
- [[_COMMUNITY_Animation Items|Animation Items]]
- [[_COMMUNITY_Async Helpers|Async Helpers]]
- [[_COMMUNITY_UWP Settings Page|UWP Settings Page]]
- [[_COMMUNITY_TypeScript Config (Node)|TypeScript Config (Node)]]
- [[_COMMUNITY_Documentation & Build Artifacts|Documentation & Build Artifacts]]
- [[_COMMUNITY_Statistics Logging|Statistics Logging]]
- [[_COMMUNITY_UWP Main Page|UWP Main Page]]
- [[_COMMUNITY_Animation Engine|Animation Engine]]
- [[_COMMUNITY_Settings Storage|Settings Storage]]
- [[_COMMUNITY_Encryption Module|Encryption Module]]
- [[_COMMUNITY_Encryptor CLI|Encryptor CLI]]
- [[_COMMUNITY_Animation Management|Animation Management]]
- [[_COMMUNITY_Linting Config|Linting Config]]
- [[_COMMUNITY_NuGet Dependencies|NuGet Dependencies]]
- [[_COMMUNITY_TypeScript References|TypeScript References]]
- [[_COMMUNITY_Encode Animations Script|Encode Animations Script]]

## God Nodes (most connected - your core abstractions)
1. `Core` - 27 edges
2. `Home` - 22 edges
3. `compilerOptions` - 18 edges
4. `AnimationsItem` - 16 edges
5. `compilerOptions` - 15 edges
6. `App` - 14 edges
7. `SettingsPage` - 12 edges
8. `ExclusiveSynchronizationContext` - 10 edges
9. `ConcentrateOnSettings` - 10 edges
10. `useTimerStore` - 10 edges

## Surprising Connections (you probably didn't know these)
- `ConcentrateOn Application` --references--> `ConcentrateOn MSIX Package v1.0.4.0`  [INFERRED]
  About.txt → ConcentrateOn/BundleArtifacts/Upload/arm.txt
- `Encryptor C# Project` --conceptually_related_to--> `ConcentrateOn Application`  [INFERRED]
  Encryptor/obj/Debug/Encryptor.csproj.FileListAbsolute.txt → About.txt
- `ARM Upload Build Manifest v1.0.4.0` --semantically_similar_to--> `ARM Build Artifacts Manifest v1.0.4.0`  [INFERRED] [semantically similar]
  ConcentrateOn/BundleArtifacts/Upload/arm.txt → ConcentrateOn/BundleArtifacts/arm.txt
- `x64 Upload Build Manifest v1.0.4.0` --semantically_similar_to--> `x64 Build Artifacts Manifest v1.0.4.0`  [INFERRED] [semantically similar]
  ConcentrateOn/BundleArtifacts/Upload/x64.txt → ConcentrateOn/BundleArtifacts/x64.txt
- `x86 Upload Build Manifest v1.0.4.0` --semantically_similar_to--> `x64 Build Artifacts Manifest v1.0.4.0`  [INFERRED] [semantically similar]
  ConcentrateOn/BundleArtifacts/Upload/x86.txt → ConcentrateOn/BundleArtifacts/x64.txt

## Import Cycles
- None detected.

## Communities (31 total, 2 thin omitted)

### Community 0 - "Web UI Components"
Cohesion: 0.10
Nodes (25): ControlButtons(), Layout(), ThemeToggle(), ThemeToggleProps, TimerDisplay(), useTimerSounds(), setLanguage(), About() (+17 more)

### Community 1 - "Animation Manifest"
Cohesion: 0.06
Nodes (32): audio, bell, bellFinish, clips, resting, startWorking, stopWorking, working (+24 more)

### Community 2 - "Core Timer Engine"
Cohesion: 0.08
Nodes (14): Animations, AnimationsItem, ConcentrateOn, Core, ConcentrateOnSettings, DateTimeOffset, Home, int (+6 more)

### Community 3 - "Web Dependencies"
Cohesion: 0.07
Nodes (28): dependencies, daisyui, i18next, react, react-dom, react-i18next, react-router-dom, tailwindcss (+20 more)

### Community 4 - "UWP Home Page"
Cohesion: 0.10
Nodes (13): ApplicationDataContainer, ConcentrateOn, App, bool, ConcentrateOnSettings, Core, double, NavigatingCancelEventArgs (+5 more)

### Community 5 - "UWP App Lifecycle"
Cohesion: 0.11
Nodes (12): Application, App, ConcentrateOn, Core, Home, EnteredBackgroundEventArgs, IActivatedEventArgs, LaunchActivatedEventArgs (+4 more)

### Community 6 - "UWP About & Donate"
Cohesion: 0.11
Nodes (12): About, ConcentrateOn, NavigationEventArgs, ConcentrateOn, NavigationEventArgs, DonatePage, ConcentrateOn, App (+4 more)

### Community 7 - "TypeScript Config (App)"
Cohesion: 0.10
Nodes (19): compilerOptions, allowArbitraryExtensions, allowImportingTsExtensions, erasableSyntaxOnly, jsx, lib, module, moduleDetection (+11 more)

### Community 8 - "Animation Items"
Cohesion: 0.16
Nodes (7): AnimationsItem, ConcentrateOn, bool, double, string, CurrentMediaPlaybackItemChangedEventArgs, MediaPlaybackList

### Community 9 - "Async Helpers"
Cohesion: 0.15
Nodes (11): AutoResetEvent, AsyncHelpers, ConcentrateOn, bool, T, ExclusiveSynchronizationContext, Func, Queue (+3 more)

### Community 10 - "UWP Settings Page"
Cohesion: 0.13
Nodes (10): ConcentrateOn, App, ConcentrateOnSettings, Core, NavigatingCancelEventArgs, NavigationEventArgs, RoutedEventArgs, SelectionChangedEventArgs (+2 more)

### Community 11 - "TypeScript Config (Node)"
Cohesion: 0.12
Nodes (16): compilerOptions, allowImportingTsExtensions, erasableSyntaxOnly, lib, module, moduleDetection, noEmit, noFallthroughCasesInSwitch (+8 more)

### Community 12 - "Documentation & Build Artifacts"
Cohesion: 0.17
Nodes (16): About.txt - Application Description, ConcentrateOn Application, Francesco Cirillo, Pomodoro Technique, Timer-Based Interval Work and Rest Cycle, ARM Build Artifacts Manifest v1.0.4.0, x64 Build Artifacts Manifest v1.0.4.0, ConcentrateOn Localization Resources (de, fr, he, ru) (+8 more)

### Community 13 - "Statistics Logging"
Cohesion: 0.16
Nodes (10): ActivityType, ConcentrateOn, DateTimeOffset, int, long, string, StatisticsLog, StatisticsLogItem (+2 more)

### Community 14 - "UWP Main Page"
Cohesion: 0.15
Nodes (7): ConcentrateOn, App, NavigationEventArgs, RoutedEventArgs, SelectionChangedEventArgs, MainPage, PointerRoutedEventArgs

### Community 15 - "Animation Engine"
Cohesion: 0.23
Nodes (10): getClipSrc(), loadAllAnimationPacks(), loadAnimationPack(), AnimationPlayer(), AnimationPlayerProps, AnimationClip, AnimationManifest, ClipState (+2 more)

### Community 16 - "Settings Storage"
Cohesion: 0.21
Nodes (7): ConcentrateOn, ConcentrateOnSettings, bool, DateTimeOffset, int, long, string

### Community 17 - "Encryption Module"
Cohesion: 0.25
Nodes (4): DllImport, Encryption, Encryptor, IntPtr

### Community 18 - "Encryptor CLI"
Cohesion: 0.31
Nodes (4): string, Encrypt_item, Encryptor, Program

### Community 19 - "Animation Management"
Cohesion: 0.40
Nodes (3): Animations, ConcentrateOn, Dictionary

### Community 20 - "Linting Config"
Cohesion: 0.33
Nodes (5): plugins, rules, react/only-export-components, react/rules-of-hooks, $schema

### Community 21 - "NuGet Dependencies"
Cohesion: 0.40
Nodes (3): Microsoft.NETCore.UniversalWindowsPlatform, Microsoft.Toolkit.Uwp.Notifications, Microsoft.Toolkit.Uwp.UI.Controls

## Knowledge Gaps
- **179 isolated node(s):** `ConcentrateOn`, `NavigationEventArgs`, `ConcentrateOn`, `Dictionary`, `ConcentrateOn` (+174 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **2 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Home` connect `UWP Home Page` to `UWP About & Donate`?**
  _High betweenness centrality (0.020) - this node is a cross-community bridge._
- **Why does `SettingsPage` connect `UWP Settings Page` to `UWP About & Donate`?**
  _High betweenness centrality (0.014) - this node is a cross-community bridge._
- **Why does `MainPage` connect `UWP Main Page` to `UWP About & Donate`?**
  _High betweenness centrality (0.011) - this node is a cross-community bridge._
- **What connects `ConcentrateOn`, `NavigationEventArgs`, `ConcentrateOn` to the rest of the system?**
  _180 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Web UI Components` be split into smaller, more focused modules?**
  _Cohesion score 0.10188261351052048 - nodes in this community are weakly interconnected._
- **Should `Animation Manifest` be split into smaller, more focused modules?**
  _Cohesion score 0.06060606060606061 - nodes in this community are weakly interconnected._
- **Should `Core Timer Engine` be split into smaller, more focused modules?**
  _Cohesion score 0.08333333333333333 - nodes in this community are weakly interconnected._