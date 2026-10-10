---
name: Low-Fi PS1 Horror
colors:
  surface: '#141313'
  surface-dim: '#141313'
  surface-bright: '#3b3939'
  surface-container-lowest: '#0f0e0e'
  surface-container-low: '#1d1b1b'
  surface-container: '#211f1f'
  surface-container-high: '#2b2a29'
  surface-container-highest: '#363434'
  on-surface: '#e6e1e1'
  on-surface-variant: '#dfbfbc'
  inverse-surface: '#e6e1e1'
  inverse-on-surface: '#323030'
  outline: '#a68a87'
  outline-variant: '#58413f'
  surface-tint: '#ffb3ad'
  primary: '#ffb3ad'
  on-primary: '#680109'
  primary-container: '#8b1e1e'
  on-primary-container: '#ff9d95'
  inverse-primary: '#aa3531'
  secondary: '#b1cada'
  on-secondary: '#1b3340'
  secondary-container: '#344c5a'
  on-secondary-container: '#a3bccc'
  tertiary: '#ecc246'
  on-tertiary: '#3d2e00'
  tertiary-container: '#cea72c'
  on-tertiary-container: '#503d00'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#ffdad6'
  primary-fixed-dim: '#ffb3ad'
  on-primary-fixed: '#410003'
  on-primary-fixed-variant: '#891c1d'
  secondary-fixed: '#cde6f7'
  secondary-fixed-dim: '#b1cada'
  on-secondary-fixed: '#031e2a'
  on-secondary-fixed-variant: '#324a57'
  tertiary-fixed: '#ffe08e'
  tertiary-fixed-dim: '#ecc246'
  on-tertiary-fixed: '#241a00'
  on-tertiary-fixed-variant: '#584400'
  background: '#141313'
  on-background: '#e6e1e1'
  surface-variant: '#363434'
  void: '#0B0A0A'
  paper: '#D8D2C4'
  blood: '#8B1E1E'
  phone-glow: '#9FB8C8'
  warn: '#C9A227'
  mold: '#3E4A3A'
typography:
  display-title:
    fontFamily: Rock Salt
    fontSize: 56px
    fontWeight: '400'
    lineHeight: 68px
  display-title-mobile:
    fontFamily: Rock Salt
    fontSize: 36px
    fontWeight: '400'
    lineHeight: 44px
  menu-item:
    fontFamily: VT323
    fontSize: 40px
    fontWeight: '400'
    lineHeight: 48px
  menu-item-mobile:
    fontFamily: VT323
    fontSize: 28px
    fontWeight: '400'
    lineHeight: 36px
  hud-stat:
    fontFamily: VT323
    fontSize: 28px
    fontWeight: '400'
    lineHeight: 32px
  hud-prompt:
    fontFamily: VT323
    fontSize: 24px
    fontWeight: '400'
    lineHeight: 28px
  hud-subtitle:
    fontFamily: VT323
    fontSize: 32px
    fontWeight: '400'
    lineHeight: 36px
  hud-subtitle-mobile:
    fontFamily: VT323
    fontSize: 22px
    fontWeight: '400'
    lineHeight: 26px
  document-heading:
    fontFamily: Special Elite
    fontSize: 24px
    fontWeight: '400'
    lineHeight: 32px
  document-body:
    fontFamily: Special Elite
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  terminal-caption:
    fontFamily: VT323
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 22px
spacing:
  gutter: 1rem
  gutter-mobile: 0.5rem
  margin: 3rem
  margin-mobile: 1rem
  space-xs: 0.25rem
  space-sm: 0.5rem
  space-md: 1rem
  space-lg: 1.5rem
  space-xl: 2.5rem
---

## Brand & Style

This design system defines the aesthetic and visual interaction framework for an Indonesian low-budget psychological horror experience set inside a silent, abandoned high school at night. Drawing heavily from late-1990s PSX software interfaces, low-resolution CRT telemetry, tactile stationery, and raw Godot Control canvas layouts, the interface prioritizes spatial dread, isolation, and unvarnished utilitarianism.

### Personality & Tone
- **Dismal & Restrained:** Horror manifests through negative space, crushing shadows, and unadorned typography rather than animated noise or gore.
- **Diegetic Utility:** Panels mimic weathered stationery, aged student council ledgers, and obsolete early-2000s mobile phone monochrome screens.
- **Deadpan Indonesian Directness:** Microcopy rejects dramatic flourishes. Text remains clinical, abrupt, and grounded in the quiet mundane (e.g., `[E] Ambil`, `Baterai HP lemah`, `DIANA TIDAK PERNAH PULANG.`).

### Aesthetic Philosophy
The visual direction fuses **Brutalist Retro-Computing** with **Tactile Analog Decay**. Interfaces rely entirely on rigid right angles, 1px to 2px structural wireframes, monochrome bitmap fonts, and stark binary states. Soft drop shadows, modern blurred glassmorphism, rounded corners, and anti-aliased floating indicators are strictly forbidden. The screen is treated as an active canvas of darkness punctuated only by dim terminal readouts and aged paper artifacts.

## Colors

The color system is constrained to a strictly enforced 6-color palette engineered to maintain visual weight and high contrast in low-light viewports. Every hue possesses an explicit mechanical role:

- **Void (`#0B0A0A`):** The primary neutral background. A crushing near-black surface that serves as the base for all menus, scene backdrops, and HUD layers.
- **Paper (`#D8D2C4`):** The default readable ink and foreground state. Represents soiled lined notebook pages, default monospace labels, inactive menu options, and primary body content.
- **Blood (`#8B1E1E`):** The singular active chromatic accent. Reserved exclusively for interactive selection focus (e.g., active menu items prefixed with `>`), critical warnings, title scratches, and terminal state failures. Only one dominant blood element may occupy a single screen at any given moment.
- **PhoneGlow (`#9FB8C8`):** Cold, phosphor-tinged blue-white. Employed specifically for mobile phone telemetry, active flashlight toggles, and healthy battery gauges (>= 10%).
- **Warn (`#C9A227`):** Amber caution signal. Restricted to critical battery alerts (< 10%), contextual inventory shortages, and low-charge hardware indications.
- **Mold (`#3E4A3A`):** Muted, decomposing dark moss green. Serves as secondary structural dividers, sub-panel borders, inactive slider tracks, and muted framing lines that recess into the darkness.

No translucent alpha gradients, radial glows, or extraneous intermediary tints are permitted.

## Typography

Typography establishes the era, technology tier, and psychological tension of the interface. Three specialized typefaces divide the system's operational domains:

1. **VT323 (Label & HUD Engine):** Used for all system telemetry, player prompts, settings toggles, device metrics, dialogue text boxes, and interactive menus. Its un-smoothed bitmap rendering evokes early game engines and CRT display terminals.
2. **Special Elite (Document & Archive Engine):** Applied strictly within in-game found artifacts—diaries, OSIS schedules, student notebooks, and physical evidence. It behaves as dry, uneven mechanical typewriter imprint on worn paper surfaces.
3. **Rock Salt (Display & Scratch Layer):** Confined to splash headers, brand signatures, and desperate pencil/chalk marks. Used sparingly to preserve visual impact.

### Rendering Rules
- **Dark Outlining:** Any text projected directly over 3D game geometry or dynamic viewports must implement a crisp 2px solid `#0B0A0A` outline (`text-shadow: 2px 0 0 #0B0A0A, -2px 0 0 #0B0A0A, 0 2px 0 #0B0A0A, 0 -2px 0 #0B0A0A;`).
- **All-Caps Convention:** HUD prompts, interaction verbs, and top-level navigation choices are universally uppercase (e.g., `MULAI`, `PENGATURAN`, `[E] AMBIL`).

## Layout & Spacing

Layouts adhere to an empty, claustrophobic philosophy built on a rigid fixed 16:9 base canvas (1920x1080 native Godot resolution) with responsive down-scaling down to mobile aspect ratios. 

### Grid & Composition Rules
- **Sparse Distribution:** The primary game screen avoids centralized dashboard boxes. Visual weight is pushed to outer perimeters, leaving 70%+ of the viewport unobstructed for immediate visual horror and spatial navigation.
- **Anchored Corners:** HUD statistics (battery, status) nest at bottom-left canvas margins (`margin: 3rem`). Interaction markers center strictly at lower thirds (`y: 80%`).
- **Padded Text Windows:** Menus rely on vertical listing hierarchies spaced by `space-sm` (8px) to `space-md` (16px), aligned flush-left to evoke early DOS launcher utilities.
- **Mobile Adaptations:** At compact screen widths (<768px), canvas margins contract to `margin-mobile` (16px), font sizes step down per the typography scale, and nested multi-column setting grids collapse into single-column vertical stacks.

## Elevation & Depth

This design system deliberately rejects 3D elevation, volumetric drop shadows, Gaussian blurs, and pseudo-skeuomorphic ambient shading. Depth is represented purely through graphic framing, opacity masks, and solid structural layering:

- **Flat Canvas Plane:** Level 0 (Base Scene). Direct full-screen 3D viewport or dithered room canvas.
- **Vignette & Grain Plate:** Level 1 (Screen Shader). A persistent 100% full-screen dithered grain overlay and subtle peripheral darkening at the highest canvas layer to bind the game world together.
- **Wireframe Panels:** Level 2 (Inspection & Pause). Panels do not cast drop shadows. Instead, they cut off background visibility via solid `#0B0A0A` fill with an explicit 1px to 2px solid border (`#3E4A3A` or `#D8D2C4`).
- **Physical Documents:** Level 3 (Note Inspection). Modals render as opaque rectangles (`#D8D2C4`) positioned over a high-opacity (85%) `#0B0A0A` backdrop mask. Contrast is maintained entirely through color juxtaposition, never through elevation shadows.

## Shapes

The geometric framework is absolute: **Sharp (`0px` radius everywhere)**. 

- **Zero Curvature:** Not a single element in the design system may feature rounded corners. All buttons, containers, tooltips, dialogue boxes, input sliders, and item frames end at crisp 90-degree right angles.
- **Borders as Geometry:** Structural definition is established exclusively with uniform 1px or 2px solid strokes. Main view containers use `#D8D2C4`, while secondary subdivisions and recessed tracks employ `#3E4A3A`.
- **Bitmap Proportions:** Small graphical glyphs (battery bars, crosshairs, switches) are built out of clean modular squares and rectangles aligned to a pixel grid, preventing subpixel interpolation and anti-aliased blur.

## Components

### Buttons & Interactive Menu Items
- **Structure:** Text-first, unbordered by default in menus; bordered by 1px solid `#3E4A3A` in isolated dialog boxes. Zero rounded corners.
- **States:**
  - *Default:* `#D8D2C4` text in `VT323`, no prefix.
  - *Focused / Active:* `#8B1E1E` (Blood) text with a prepended `> ` character indicator (e.g., `> MULAI`).
  - *Pressed:* Reverse-video block fill where background turns `#8B1E1E` and text turns `#0B0A0A`.
  - *Disabled:* Muted `#3E4A3A` text without prefix.

### Interaction HUD Prompts
- **Structure:** Floating, minimal single-line prompt positioned at bottom-center of the screen.
- **Styling:** `VT323`, 24px, uppercase. Text in `#D8D2C4` with a mandatory 2px solid `#0B0A0A` outline. Keybinds enclosed in square brackets: `[E] BUKA PINTU`, `[SPASI] TAHAN NAPAS`.

### Checkboxes & Segmented Toggles
- **Structure:** Bracketed binary text states. No modern sliding pills.
- **Display:** Rendered as `[ON]  OFF` or `ON  [OFF]`. The active state is encased in square brackets in `#D8D2C4` or highlighted with `#8B1E1E`, while the inactive option is displayed in `#3E4A3A`.

### Sliders (Volume, Brightness)
- **Structure:** Discontinuous stepped pixel blocks rather than continuous fluid bars.
- **Composition:** A row of 10 to 12 solid square glyphs (`■`). Active filled steps use `#D8D2C4` (or `#8B1E1E` if the slider row has focus); inactive steps remain hollow (`□`) or use `#3E4A3A`.

### Note / Document Viewer Card
- **Structure:** Centralized textured rectangle (`#D8D2C4`) set against a darkened backdrop (`#0B0A0A` at 85% opacity).
- **Styling:** Text formatted in `Special Elite`, color `#0B0A0A`. Hard 0px borders. Margin notes or scratched clues utilize `#8B1E1E`. Bottom footer displays a minimal return indicator in `VT323`: `[E] TUTUP`.

### Phone Screen HUD (Device Overlay)
- **Structure:** A vertical rectangular frame mimicking an early budget mobile device. Sharp 1px `#3E4A3A` outer border.
- **Styling:** Dark `#0B0A0A` screen fill with cold `#9FB8C8` (PhoneGlow) typography and pixel icons. Top status line contains a 24-hour clock (`23:47`) and battery status block.
- **Battery Indicator:** Pixel-stepped battery container. Fills with `#9FB8C8` during normal power (>=10%) and shifts to `#C9A227` (Warn) when dropped below 10%.

### Subtitle Banner
- **Structure:** Frameless floating typography placed directly above the interaction zone.
- **Styling:** `VT323`, 32px, text-color `#D8D2C4`, with a heavy 2px `#0B0A0A` shadow outline to ensure legibility across deep school environments and flashlight beams.