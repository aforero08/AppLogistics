// Global mappings shim to redirect legacy static AutoMapper usage to LegacyMapper.
// Provides alias so existing tests using Mapper.Map<T>() continue to work without edits.
global using Mapper = AppLogistics.Mapping.LegacyMapper;
global using AppLogistics.Mapping;
