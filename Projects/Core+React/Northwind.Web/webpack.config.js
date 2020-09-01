const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
module.exports = {
    //mode: "production",
    mode: "development",
    entry: {
        //script.
        compiledScript: "./Scripts/index.tsx",

        //scss.
        northwind: "./wwwroot/scss/custom/northwind.scss",
        kendoThemeBootstrap: "./wwwroot/scss/vendor/kendo-theme-bootstrap.scss",
    },
    output: {
        filename: "[name].js",
        path: path.resolve(__dirname, "../Northwind.Web.CDN/wwwroot/js/dist")
    },

    // Enable sourcemaps for debugging webpack's output.
    devtool: "source-map",

    resolve: {
        // Add '.ts' and '.tsx' as resolvable extensions.
        extensions: [".ts", ".tsx", ".js", ".json"]
    },

    module: {
        rules: [
            // All files with a '.ts' or '.tsx' extension will be handled by 'awesome-typescript-loader'.
            {
                test: /\.tsx?$/,
                loader: "awesome-typescript-loader"
            },

            {
                test: [/\.css$/, /\.scss$/],
                // use: ['style-loader', 'css-loader', 'sass-loader']
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true
                        }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sourceMap: true
                        }
                    }
                ]
            },
            // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
            {
                enforce: "pre",
                test: /\.js$/,
                loader: "source-map-loader"
            },
            {
                test: /\.(png|jpg|gif|svg)$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[path][name].[ext]',
                        // context: path.resolve(__dirname, "../Northwind.Web.CDN/"),
                        outputPath: 'img/',
                        publicPath: 'img/',
                        // useRelativePaths: true
                    }
                }]
            },
        ]
    },
    plugins: [new MiniCssExtractPlugin({
        filename: "../../css/[name].css"
    })],

    // When importing a module whose path matches one of the following, just
    // assume a corresponding global variable exists and use that instead.
    // This is important because it allows us to avoid bundling all of our
    // dependencies, which allows browsers to cache those libraries between builds.
    externals: {
        "react": "React",
        "react-dom": "ReactDOM"
    }
};