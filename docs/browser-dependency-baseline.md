# Browser dependency baseline

This document records the browser dependency state before moving checked-in third-party assets under package management. It is a point-in-time baseline from 2026-08-26, not a dependency-upgrade plan.

Phase 0 intentionally makes no runtime dependency, source-asset, or bundle-order changes. Later phases should first reproduce the current behavior from managed packages and only then upgrade library versions.

## Phase 1 acquisition result

Phase 1 moved the exact jQuery 3.3.1, jQuery Validation 1.17.0, jQuery Validation Unobtrusive 3.2.11, and jQuery Timepicker Addon 1.6.3 distributions into `package.json` without upgrading them. Production bundles now read those sources directly from `node_modules`; `npm run build` also creates ignored, unminified `wwwroot/*/Dependencies` copies for Development pages.

The application-owned Globalize and date/time culture adapters remain tracked. The custom jQuery UI build, Bootstrap assets, Font Awesome assets, MVC components, and all Phase 0 discrepancies remain unchanged for later phases.

After these packages were declared, `npm audit --package-lock-only` reported three affected package entries: the existing high-severity `brace-expansion` build-tool finding, a moderate-severity jQuery finding, and a high-severity jQuery Validation finding. This added visibility is an intended result of Phase 1. Resolving the browser-library findings requires version upgrades and regression testing, so those behavior-changing updates remain deferred to a later phase.

## Current build and delivery model

- `src/AppLogistics.Web/package.json` declares the build tools plus the exact-version Phase 1 browser packages.
- `src/AppLogistics.Web/bundle.js` reads Phase 1 sources directly from `node_modules`; deferred browser libraries continue to come from `wwwroot`.
- Development pages load generated Phase 1 dependency copies and the remaining tracked individual source files from the public and private layouts.
- Staging and production pages load generated public/private vendor and site bundles.
- Generated bundles are ignored by Git and rebuilt by `npm run build`; publish runs both `npm ci` and `npm run build`.
- The third-party browser footprint identified below contains 43 tracked JavaScript, CSS, font, and image files. Application CSS, scripts, and images are outside this count.

## Managed npm baseline

The baseline was reproduced with Node.js 24.12.0, npm 11.6.2, esbuild 0.28.1, and glob 13.0.6:

```powershell
Push-Location src/AppLogistics.Web
npm ci
npm run build
npm audit --package-lock-only
Pop-Location
```

On 2026-08-26, `npm audit --package-lock-only` reported one high-severity finding: `brace-expansion` 5.0.8 through `glob`, [GHSA-rgw5-rvv9-x895](https://github.com/advisories/GHSA-rgw5-rvv9-x895). npm reported that a fix was available. This is build-time tooling, not code shipped in the browser bundles, but it should be resolved or explicitly assessed separately from the browser-library migration.

The audit does not inspect any of the checked-in browser libraries listed below. A clean npm audit must not be interpreted as a clean browser dependency inventory until those libraries are declared packages or covered by another scanner.

## Browser library inventory

"Exact" comparisons below ignore CRLF/LF line-ending differences. Package comparisons used the published package at the stated version. NuGet comparisons used the version restored in the local global package cache.

| Library | Runtime snapshot | Tracked location | Upstream comparison | Declared license | Phase 1 classification |
| --- | --- | --- | --- | --- | --- |
| jQuery | 3.3.1 | `wwwroot/Scripts/JQuery/jquery.js` | Exact match for `jquery@3.3.1` | MIT | Safe exact-version npm candidate |
| jQuery Validation | 1.17.0 | `wwwroot/Scripts/JQuery/jquery.validate.js` | Exact match for `jquery-validation@1.17.0` | MIT | Safe exact-version npm candidate |
| jQuery Validation Unobtrusive | 3.2.11 | `wwwroot/Scripts/JQuery/jquery.validate.unobtrusive.js` | Exact match for `jquery-validation-unobtrusive@3.2.11` | Apache 2.0 header | Safe exact-version npm candidate |
| Globalize | Unversioned legacy snapshot | `wwwroot/Scripts/JQuery/jquery.globalize.js` | Does not match the available npm 0.1.0 prereleases or 0.1.1 | Local header declares MIT or GPL v2 | Hold for provenance and compatibility work |
| Globalize culture adapters | Application-specific `en` and `es` objects | `wwwroot/Scripts/JQuery/Cultures` | Custom wrappers around the application's `window.cultures` contract | No separate header | Keep as application-owned adapters |
| jQuery UI | 1.12.1 custom build | `wwwroot/Scripts/JQueryUI/jquery-ui.js` | Not the `jquery-ui-dist@1.12.1` distribution; contains Keycode, Datepicker, Effects, Fade, and Slide components | MIT | Recreate the component selection before moving |
| jQuery UI theme | 1.12.1 customized theme | `wwwroot/Content/JQueryUI` and `wwwroot/Images/JQueryUI` | Partial/customized CSS with repository-specific image URLs | MIT | Move only with CSS/image output verification |
| jQuery Timepicker Addon | 1.6.3 | `wwwroot/Scripts/JQueryUI/jquery-ui.timepicker-addon.js` and matching CSS | Exact matches for `jquery-ui-timepicker-addon@1.6.3` | MIT | Safe exact-version npm candidate; keep culture adapters local |
| Date/time culture adapters | Application-specific `en` and `es` objects | `wwwroot/Scripts/JQueryUI/Cultures` | Custom combined datepicker/timepicker settings | No separate header | Keep as application-owned adapters |
| Bootstrap CSS | 4.3.1 | `wwwroot/Content/Bootstrap/bootstrap.css` | Matches `bootstrap@4.3.1` except the checked-in copy omits the source-map comment | MIT | Low-risk npm candidate with output verification |
| Bootstrap Native | 2.0.25 Bootstrap 4 build | `wwwroot/Scripts/Bootstrap/bootstrap-native.js` | Based on `bootstrap.native@2.0.25/dist/bootstrap-native-v4.js`, with a local parenthesized `tabindex` fix near line 939 | MIT | Preserve the local fix explicitly or upgrade separately |
| Font Awesome Free | 5.15.4 | `wwwroot/Content/FontAwesome` | Font binaries exactly match npm; CSS is reformatted and the font declarations were reduced to WOFF2 with repository-specific URLs | CC BY 4.0, OFL 1.1, and MIT | npm candidate only after font URL/output verification |
| MVC Grid | 7.0.0 | `wwwroot/Scripts/MvcGrid`, `wwwroot/Content/MvcGrid`, and related cultures | Paired with `NonFactors.Grid.Mvc6` 7.0.0; JS differences are formatting, CSS changes include the local font URL, and the font binary matches | MIT | Treat as NuGet-coupled assets, not an independent npm upgrade |
| MVC Lookup | Browser assets 6.0.0; NuGet 6.3.0 | `wwwroot/Scripts/MvcLookup`, `wwwroot/Content/MvcLookup`, and related cultures | JS exactly matches the 6.0.0 package, not the referenced 6.3.0 NuGet package; CSS is also based on 6.0.0 | MIT | Resolve the 6.0/6.3 mismatch before changing acquisition |
| MVC Tree | Application-owned | `wwwroot/Scripts/MvcTree`, `wwwroot/Content/MvcTree`, and `wwwroot/Images/MvcTree` | Implemented alongside application tag helpers and models; no external package was identified | No separate third-party license | Keep as application source |

No root-level third-party license or notice files were found. The checked-in distributions rely primarily on embedded headers. Package migration must retain any license notices required in source distributions or generated bundles.

## Known security and support observations

This is a triage baseline, not proof that a vulnerability is exploitable in AppLogistics.

- jQuery 3.3.1 is in the affected range for two moderate HTML-manipulation XSS advisories fixed in 3.5.0: [GHSA-gxr4-xjj5-5px2](https://github.com/jquery/jquery/security/advisories/GHSA-gxr4-xjj5-5px2) and [GHSA-jpcq-cgw6-v4j6](https://github.com/jquery/jquery/security/advisories/GHSA-jpcq-cgw6-v4j6). Exploitation depends on untrusted HTML reaching affected APIs.
- The custom jQuery UI build contains Datepicker and is in the affected range for its `altField` and `*Text` option advisories: [GHSA-9gj3-hwp5-pmwc](https://github.com/jquery/jquery-ui/security/advisories/GHSA-9gj3-hwp5-pmwc) and [GHSA-j7qv-pgf6-hvh4](https://github.com/jquery/jquery-ui/security/advisories/GHSA-j7qv-pgf6-hvh4). Current application initialization uses fixed selectors and localized settings, so Phase 0 did not establish an exploit path.
- [Bootstrap 4 reached end of life](https://getbootstrap.com/docs/4.6/end-of-life/) on 2023-01-01 and no longer receives ordinary security fixes. The final Bootstrap 4 release or a Bootstrap 5 migration should be considered in a later, behavior-changing phase.
- The npm registry marks the exact jQuery 3.3.1 and Bootstrap 4.3.1 packages as deprecated/unsupported.
- Globalize, the timepicker addon, Bootstrap Native, and the custom jQuery UI build need explicit maintenance decisions because acquisition alone will not modernize them.

## Baseline discrepancies to preserve or resolve

1. `Mvc.Lookup` browser assets are version 6.0.0 while `NonFactors.Lookup.Mvc6` is version 6.3.0. Do not silently replace the browser files as part of a package-management-only change.
2. Bootstrap Native contains a one-line local correction relative to the published Bootstrap 4 distribution. A direct package path substitution would lose it.
3. jQuery UI is a selected-component build, not the full distribution. Switching to the full npm distribution would change bundle size and potentially behavior.
4. Font Awesome CSS intentionally ships only WOFF2 assets and uses repository-specific paths.
5. `Views/Shared/_PublicLayout.cshtml` references `Scripts/JQuery/Cultures/globalize.lt.js` in Development, but that file is not present. This pre-existing development-only 404 should be resolved separately rather than hidden inside the acquisition migration.

## Generated asset hash baseline

The SHA-256 manifest in [`browser-assets-baseline.sha256`](browser-assets-baseline.sha256) was generated after a clean `npm ci` and successful `npm run build`. It covers ignored public/private bundles, emitted fonts and images, and minified application assets.

Hash equality is a strong check for an acquisition-only change. A hash difference is not automatically a defect, because equivalent upstream CSS or a bundler path change can produce different bytes, but every difference must be explained and followed by browser verification.

## Browser smoke-test checklist

Run this checklist in Development, then repeat the production-specific checks against Staging or Production-style bundles. Exercise both English and Spanish where applicable.

### Build and delivery

- [ ] `npm ci` completes using the committed lockfile.
- [ ] `npm run build` completes without missing entry points or asset resolution errors.
- [ ] A second build produces the same baseline asset hashes when no source or tool version changed.
- [ ] Development pages load individual source assets without unexpected console errors or 404 responses.
- [ ] Staging/Production pages load versioned `vendor.min` and `site.min` assets without unexpected console errors or 404 responses.
- [ ] Fonts, jQuery UI theme images, navigation images, and MVC Tree images render from their emitted URLs.

### Public experience

- [ ] Login renders correctly and submits with valid credentials.
- [ ] Required-field and invalid-value messages appear without a full page submission.
- [ ] Password recovery renders, validates, and submits.
- [ ] Language selection works where exposed.
- [ ] Public dropdowns, tooltips, alerts, and Font Awesome icons render and behave correctly.

### Authenticated shell

- [ ] Header language dropdown opens, selects a language, and closes correctly.
- [ ] Navigation search filters and restores menu items.
- [ ] Breadcrumbs, alerts, responsive layout, and icons render correctly.
- [ ] Read-only lookup and tree controls initialize without console errors.

### Forms, localization, and widgets

- [ ] Create and edit a conventional reference item such as a Vehicle Type or Document Type.
- [ ] Required, length, numeric, range, and server-returned validation messages display correctly.
- [ ] English numeric and date values parse and validate correctly.
- [ ] Spanish numeric and date values parse and validate correctly.
- [ ] Employee birth-date selection and validation work.
- [ ] Service Report start/end datepickers open, navigate, select dates, and submit.
- [ ] At least one date-time picker opens, selects time, and updates its input.
- [ ] Tooltips and Bootstrap dropdowns work with keyboard and pointer input.

### MVC components

- [ ] A grid loads, sorts, filters, paginates, and reloads after an action.
- [ ] English and Spanish grid labels render correctly.
- [ ] A single-select lookup searches, selects, clears, and restores a value.
- [ ] A multi-select lookup searches, selects multiple rows, saves, and cancels.
- [ ] English and Spanish lookup labels render correctly.
- [ ] The Roles permission tree expands, collapses, selects, deselects, saves, and renders read-only state.

## Phase 0 completion criteria

- The dependency inventory and upstream comparisons are recorded.
- Known local modifications and version mismatches are identified.
- The current npm audit result is recorded separately from vendored-browser risk.
- Current generated outputs have a reproducible hash manifest.
- Manual regression coverage is defined before acquisition or upgrade work begins.
- No browser dependency version or runtime behavior was intentionally changed.
