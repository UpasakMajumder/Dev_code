/* Configuration */
const config = require('./config');
const DEVELOPMENT = process.env.isDevelopment;

/* Modules */
const path = require('path');
const webpack = require('webpack');
const WriteFilePlugin = require('write-file-webpack-plugin');
const eslintConfig = require('eslint-config-actum').getConfig({
    environment: {
        isDevelopment: DEVELOPMENT
    }
});

/* Plugins for Webpack */
const pluginsCollection = {
    /* Plugins common for each environment */
    common: [
        /* Declare Node environment within Webpack */
        new webpack.DefinePlugin({
            'process.env': {
                'NODE_ENV': JSON.stringify(process.env.NODE_ENV),
                'isDevelopment': DEVELOPMENT
            }
        })
    ],

    /* Environment-specific plugins */
    development: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.optimize.LimitChunkCountPlugin({
            maxChunks: DEVELOPMENT ? 10 : 20
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name: 'common',
            filename: '[name].js',
            minChunks: Infinity
        }),
        new WriteFilePlugin({
            log: false
        })
    ],

    production: [
        new webpack.optimize.OccurrenceOrderPlugin(true),
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false
            },
            sourceMap: true,
            mangle: false,
            comments: false
        }),
        new webpack.NoErrorsPlugin()
    ]
};

/* Concat common and unique plugins */
const plugins = pluginsCollection.common.concat(pluginsCollection[process.env.NODE_ENV]);

/* Get dynamic entry name from the one set in config.js@JS_ENTRY */
const APP_ENTRY_NAME = path.parse(config.JS_ENTRY).name;

module.exports = {
    entry: {
        [APP_ENTRY_NAME]: ['babel-polyfill', 'whatwg-fetch', config.JS_ENTRY],
        common: ['react', 'react-dom', 'redux']
    },
    output: {
        path: path.resolve(process.cwd(), config.JS_BUILD),
        publicPath: '/js/',
        filename: DEVELOPMENT ? '[name].js' : '[name].min.js',
        chunkFilename: 'chunks/[name].js'
    },
    cache: true,
    devtool: DEVELOPMENT && 'cheap-eval-source-map',
    plugins,
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                include: [path.resolve(process.cwd(), config.JS_BASE)],
                loaders: [
                    {
                        loader: 'react-hot-loader'
                    },
                    {
                        loader: 'babel-loader',
                        query: { cacheDirectory: true }
                    },
                    {
                        loader: 'eslint-loader',
                        options: {
                          configFile: eslintConfig,
                          rules: {
                            "indent": ["error", 2]
                          }
                        }
                    }
                ]
            },
            {
                test: /\.svg$/,
                loaders: [
                    {
                        loader: 'babel-loader'
                    },
                    {
                        loader: 'react-svg-loader',
                        query: { jsx: true }
                    }
                ]
            }
        ]
    },
    resolve: {
        alias: {
            components: path.resolve(process.cwd(), `${config.JS_BASE}/components`),
            reducers: path.resolve(process.cwd(), `${config.JS_BASE}/store/reducers`),
            utilities: path.resolve(process.cwd(), `${config.JS_BASE}/utilities`),
            services: path.resolve(process.cwd(), `${config.JS_BASE}/utilities/service.map.js`)
        }
    }
};
