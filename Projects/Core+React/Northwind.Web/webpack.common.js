const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
module.exports = {
      entry: {
        //advanceSearch: "./index.tsx",
        dialog: "./Scripts/Common/Dialog/index.tsx",
        kendoGrid: "./Scripts/Common/Grid/index.tsx",
        multiselectpanel: "./Scripts/Common/MultiSelectPanel/index.tsx",
        documentmanagement: "./Scripts/Common/DocumentManager/index.tsx",
        contextMenu: "./Scripts/Common/ContextMenu/ContextMenu.tsx",
        addPolicy: "./Scripts/Component/Policy/AddPolicy.tsx",
        notificationSummary: "./Scripts/Component/NotificationSummary/index.tsx",
        messageInbox: "./Scripts/Component/MessageInbox/index.tsx",
        distributionList: "./Scripts/Component/DistributionList/index.tsx",
        distributionListDialog: "./Scripts/Component/DistributionList/DistributionListDialog.tsx",
        employeeDirectory: "./Scripts/Component/EmployeeDirectory/Index.tsx",
        officegrid: "./Scripts/Component/OfficeDirectory/index.tsx",
        fileUpload: "./Scripts/Component/FileUpload/index.tsx",
        folderTree: "./Scripts/Component/FolderTree/index.tsx",
        switchuser: "./Scripts/Component/SwitchUser/index.tsx",
        revenueRecognition: "./Scripts/Component/RevenueRecognition/Index.tsx",
        userGroupDetails: "./Scripts/Component/GroupManagement/index.tsx",
        northwind: "./wwwroot/scss/custom/northwind.scss",
        kendoThemeBootstrap: "./wwwroot/scss/vendor/kendo-theme-bootstrap.scss",
        //farContractTypeClause: "./Scripts/Component/farContractTypeClause/index.tsx"
        farClause: "./Scripts/Component/farClause/index.tsx"
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
    }), new CleanWebpackPlugin(),
        new HtmlWebpackPlugin({
            title: 'Production',
        }),
        new BundleAnalyzerPlugin()],

    // When importing a module whose path matches one of the following, just
    // assume a corresponding global variable exists and use that instead.
    // This is important because it allows us to avoid bundling all of our
    // dependencies, which allows browsers to cache those libraries between builds.
    externals: {
        "react": "React",
        "react-dom": "ReactDOM"
    }
};