const fs = require("fs");
const glob = require("glob");
const path = require("path");
const esbuild = require("esbuild");
const started = process.hrtime();

glob.sync("./wwwroot/Scripts/**/*.min.js").forEach(fs.unlinkSync);
glob.sync("./wwwroot/Content/**/*.min.css").forEach(fs.unlinkSync);

copy([
    ["./node_modules/jquery/dist/jquery.js", "./wwwroot/Scripts/Dependencies/jquery.js"],
    ["./node_modules/jquery-validation/dist/jquery.validate.js", "./wwwroot/Scripts/Dependencies/jquery.validate.js"],
    ["./node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js", "./wwwroot/Scripts/Dependencies/jquery.validate.unobtrusive.js"],
    ["./node_modules/jquery-ui-timepicker-addon/dist/jquery-ui-timepicker-addon.js", "./wwwroot/Scripts/Dependencies/jquery-ui.timepicker-addon.js"],
    ["./node_modules/jquery-ui-timepicker-addon/dist/jquery-ui-timepicker-addon.css", "./wwwroot/Content/Dependencies/jquery-ui.timepicker-addon.css"]
]);

const vendorPrivateJs = bundle([
    "./node_modules/jquery/dist/jquery.js",
    "./node_modules/jquery-validation/dist/jquery.validate.js",
    "./node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js",
    "./wwwroot/Scripts/jquery/jquery.globalize.js",
    "./wwwroot/Scripts/jquery/Cultures/globalize.en.js",
    "./wwwroot/Scripts/jquery/Cultures/globalize.es.js",
    "./wwwroot/Scripts/jqueryui/jquery-ui.js",
    "./wwwroot/Scripts/jqueryui/Cultures/jquery-ui.en.js",
    "./wwwroot/Scripts/jqueryui/Cultures/jquery-ui.es.js",
    "./node_modules/jquery-ui-timepicker-addon/dist/jquery-ui-timepicker-addon.js",
    "./wwwroot/Scripts/bootstrap/bootstrap-native.js",
    "./wwwroot/Scripts/mvclookup/mvc-lookup.js",
    "./wwwroot/Scripts/mvclookup/Cultures/mvc-lookup.en.js",
    "./wwwroot/Scripts/mvclookup/Cultures/mvc-lookup.es.js",
    "./wwwroot/Scripts/mvcgrid/mvc-grid.js",
    "./wwwroot/Scripts/mvcgrid/Cultures/mvc-grid.en.js",
    "./wwwroot/Scripts/mvcgrid/Cultures/mvc-grid.es.js",
    "./wwwroot/Scripts/mvctree/*.js",
    "./wwwroot/Scripts/shared/widgets/alerts.js",
    "./wwwroot/Scripts/shared/widgets/datepicker.js",
    "./wwwroot/Scripts/shared/widgets/grid.js",
    "./wwwroot/Scripts/shared/widgets/header.js",
    "./wwwroot/Scripts/shared/widgets/tree.js",
    "./wwwroot/Scripts/shared/widgets/lookup.js",
    "./wwwroot/Scripts/shared/widgets/navigation.js",
    "./wwwroot/Scripts/shared/widgets/validator.js",
    "./wwwroot/Scripts/shared/widgets/number.js",
    "./wwwroot/Scripts/shared/widgets/tooltip.js"
], "./wwwroot/Scripts/Private/vendor.min.js");

const sitePrivateJs = bundle([
    "./wwwroot/Scripts/shared/private.js"
], "./wwwroot/Scripts/Private/site.min.js");

const vendorPublicJs = bundle([
    "./node_modules/jquery/dist/jquery.js",
    "./node_modules/jquery-validation/dist/jquery.validate.js",
    "./node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js",
    "./wwwroot/Scripts/jquery/jquery.globalize.js",
    "./wwwroot/Scripts/jquery/Cultures/globalize.en.js",
    "./wwwroot/Scripts/jquery/Cultures/globalize.es.js",
    "./wwwroot/Scripts/bootstrap/bootstrap-native.js",
    "./wwwroot/Scripts/shared/widgets/alerts.js",
    "./wwwroot/Scripts/shared/widgets/validator.js"
], "./wwwroot/Scripts/Public/vendor.min.js");

const sitePublicJs = bundle([
    "./wwwroot/Scripts/shared/public.js"
], "./wwwroot/Scripts/Public/site.min.js");

const appJs = minify(["./wwwroot/Scripts/application/**/*.js"]);

const vendorPrivateCss = bundle([
    "./node_modules/jquery-ui-timepicker-addon/dist/jquery-ui-timepicker-addon.css",
    "./wwwroot/Content/jqueryui/jquery-ui.css",
    "./wwwroot/Content/bootstrap/*.css",
    "./wwwroot/Content/fontawesome/*.css",
    "./wwwroot/Content/mvcgrid/*.css",
    "./wwwroot/Content/mvctree/*.css",
    "./wwwroot/Content/mvclookup/*.css"
], "./wwwroot/Content/Private/vendor.min.css");

const sitePrivateCss = bundle([
    "./wwwroot/Content/Shared/alerts.css",
    "./wwwroot/Content/Shared/content.css",
    "./wwwroot/Content/Shared/header.css",
    "./wwwroot/Content/Shared/navigation.css",
    "./wwwroot/Content/Shared/overrides.css",
    "./wwwroot/Content/Shared/table.css",
    "./wwwroot/Content/Shared/widget-box.css",
    "./wwwroot/Content/Shared/private.css"
], "./wwwroot/Content/Private/site.min.css");

const vendorPublicCss = bundle([
    "./wwwroot/Content/bootstrap/*.css",
    "./wwwroot/Content/fontawesome/*.css"
], "./wwwroot/Content/Public/vendor.min.css");

const sitePublicCss = bundle([
    "./wwwroot/Content/Shared/alerts.css",
    "./wwwroot/Content/Shared/content.css",
    "./wwwroot/Content/Shared/overrides.css",
    "./wwwroot/Content/Shared/public.css"
], "./wwwroot/Content/Public/site.min.css");

const appCss = minify(["./wwwroot/Content/application/**/*.css"]);

Promise.all([
    vendorPrivateJs, 
    sitePrivateJs, 
    vendorPublicJs, 
    sitePublicJs, 
    ...appJs, 
    vendorPrivateCss, 
    sitePrivateCss, 
    vendorPublicCss, 
    sitePublicCss, 
    ...appCss
]).then(_ => {
    const ended = process.hrtime(started);

    console.log("Bundled in: \x1b[32m%ds %dms\x1b[0m", ended[0], ended[1] / 1000000);
});

function bundle(files, outFile) {
    return esbuild.build({
        entryPoints: [...new Set(files.map(pattern => glob.sync(pattern)).flat())],
        outdir: "./tmp",
        minify: true,
        write: false
    }).then(result => {
        const dir = path.dirname(outFile);

        if (!fs.existsSync(dir)) {
            fs.mkdirSync(dir, { recursive: true });
        }

        const bundle = fs.openSync(outFile, "w");

        result.outputFiles.forEach(file => {
            fs.writeSync(bundle, file.text);
        });

        fs.closeSync(bundle);
    });
}

function minify(files) {
    return [...new Set(files.map(pattern => glob.sync(pattern)).flat())].map(file => esbuild.build({
        entryPoints: [file],
        outExtension: { ".js": ".min.js", ".css": ".min.css" },
        outdir: path.dirname(file),
        minify: true
    }));
}

function copy(files) {
    files.forEach(([source, destination]) => {
        fs.mkdirSync(path.dirname(destination), { recursive: true });
        fs.copyFileSync(source, destination);
    });
}
