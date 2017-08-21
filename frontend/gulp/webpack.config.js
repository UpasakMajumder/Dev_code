/* Configuration */
const config = require('./config');
const DEVELOPMENT = process.env.isDevelopment !== 'false';

/* Modules */
const path = require('path');
const webpack = require('webpack');
const WriteFilePlugin = require('write-file-webpack-plugin');
const eslintConfig = require('eslint-config-actum').getConfig({ environment: false });
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

/* Plugins for Webpack */
const pluginsCollection = {
  /* Plugins common for each environment */
  common: [
    /* Declare Node environment within Webpack */
    new webpack.DefinePlugin({
      'process.env': {
        'NODE_ENV': JSON.stringify(process.env.NODE_ENV),
        'isDevelopment': JSON.stringify(DEVELOPMENT)
      }
    })
  ],

  /* Environment-specific plugins */
  development: [
    new webpack.HotModuleReplacementPlugin(),
    new webpack.optimize.LimitChunkCountPlugin({
      maxChunks: 1
    }),
    new webpack.optimize.CommonsChunkPlugin({
      name: 'common',
      filename: '[name].js',
      minChunks: 0
    }),
    new WriteFilePlugin({
      log: false
    }),
    // new BundleAnalyzerPlugin()
  ],
  production: [
    new webpack.optimize.OccurrenceOrderPlugin(true),
    new webpack.optimize.UglifyJsPlugin({
      compress: {
        warnings: false
      },
      sourceMap: false,
      comments: false
    }),
    new webpack.optimize.LimitChunkCountPlugin({
      maxChunks: 10
    }),
    new webpack.optimize.CommonsChunkPlugin({
      name: 'common',
      filename: '[name].min.js',
      minChunks: Infinity
    }),
    new webpack.NoEmitOnErrorsPlugin()
  ]
};

/* Concat common and unique plugins */
const plugins = pluginsCollection.common.concat(pluginsCollection[process.env.NODE_ENV]);

/* Get dynamic entry name from the one set in config.js@JS_ENTRY */
const APP_ENTRY_NAME = path.parse(config.JS_ENTRY).name;

module.exports = {
    entry: {
        [APP_ENTRY_NAME]: ['babel-polyfill', 'whatwg-fetch', config.JS_ENTRY],
        common: ['react', 'react-dom', 'redux', 'popper.js', 'react-tippy', 'lodash/isEqual']
    },
    output: {
        path: path.resolve(process.cwd(), config.JS_BUILD),
        publicPath: '/js/',
        filename: DEVELOPMENT ? '[name].js' : '[name].min.js',
        chunkFilename: 'chunks/[name].js'
    },
    cache: true,
    devtool: DEVELOPMENT && 'eval',
    plugins,
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                include: [path.resolve(process.cwd(), config.JS_BASE)],
                loaders: [
                    {
                        loader: 'babel-loader',
                        query: { cacheDirectory: true }
                    },
                    {
                        loader: 'eslint-loader',
                        options: {
                          configFile: eslintConfig,
                          rules: {
                            "indent": ["error", 2],
                            "no-shadow": 0,
                            "new-cap": 0, // immutable.js
                            "import/no-unresolved": 0, // webpack aliases
                            "import/extensions": 0  // webpack aliases
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
            },
            {
              test: /\.css$/,
              use: [
                { loader: "style-loader" },
                { loader: "css-loader" },
              ],
            },
            {
              test: /\.(jpe?g|png|gif)$/i,
              loaders: ['file-loader?context=src/images&name=images/[path][name].[ext]', {
                loader: 'image-webpack-loader',
                query: {
                  mozjpeg: {
                    progressive: true,
                  },
                  gifsicle: {
                    interlaced: false,
                  },
                  optipng: {
                    optimizationLevel: 4,
                  },
                  pngquant: {
                    quality: '75-90',
                    speed: 3,
                  },
                }
              }]
            }
        ]
    },
    resolve: {
        alias: {
            'app.dump': path.resolve(process.cwd(), `${config.JS_BASE}/components/_dump`),
            'app.smart': path.resolve(process.cwd(), `${config.JS_BASE}/components`),
            'app.ac': path.resolve(process.cwd(), `${config.JS_BASE}/AC`),
            'app.globals': path.resolve(process.cwd(), `${config.JS_BASE}/globals.js`),
            'app.consts': path.resolve(process.cwd(), `${config.JS_BASE}/constants.js`),
            'app.reducers': path.resolve(process.cwd(), `${config.JS_BASE}/reducers`),
            'app.store': path.resolve(process.cwd(), `${config.JS_BASE}/store`),
            'app.helpers': path.resolve(process.cwd(), `${config.JS_BASE}/helpers`),
            'app.ws': path.resolve(process.cwd(), `${config.JS_BASE}/AC/_ws`),
            'app.gfx': path.resolve(process.cwd(), config.GFX_BASE)
        }
    }
};
