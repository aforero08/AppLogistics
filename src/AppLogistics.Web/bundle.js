const fs = require("fs");
const glob = require("glob");
const path = require("path");
const esbuild = require("esbuild");
const started = process.hrtime();

glob.sync("./wwwroot/Scripts/**/*.min.js").forEach(fs.unlinkSync);
glob.sync("./wwwroot/Content/**/*.min.css").forEach(fs.unlinkSync);

const vendorPrivateJs = bundle([
    "./wwwroot/Scripts/jquery/jquery.js",
    "./wwwroot/Scripts/jquery/**/*.js",
    "./wwwroot/Scripts/jqueryui/jquery-ui.js",
    "./wwwroot/Scripts/jqueryui/**/*.js",
    "./wwwroot/Scripts/mvclookup/**/*.js",
    "./wwwroot/Scripts/mvcgrid/**/*.js",
    "./wwwroot/Scripts/mvctree/*.js",
    "./wwwroot/Scripts/bootstrap/*.js",
    "./wwwroot/Scripts/shared/widgets/*.js"
], "./wwwroot/Scripts/Private/vendor.min.js");

const sitePrivateJs = bundle([
    "./wwwroot/Scripts/shared/private.js"
], "./wwwroot/Scripts/Private/site.min.js");

const vendorPublicJs = bundle([
    "./wwwroot/Scripts/jquery/jquery.js",
    "./wwwroot/Scripts/jquery/**/*.js",
    "./wwwroot/Scripts/bootstrap/*.js",
    "./wwwroot/Scripts/shared/widgets/validator.js",
    "./wwwroot/Scripts/shared/widgets/alerts.js"
], "./wwwroot/Scripts/Public/vendor.min.js");

const sitePublicJs = bundle([
    "./wwwroot/Scripts/shared/public.js"
], "./wwwroot/Scripts/Public/site.min.js");

const appJs = minify(["./wwwroot/Scripts/application/**/*.js"]);

const vendorPrivateCss = bundle([
    "./wwwroot/Content/jqueryui/*.css",
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
