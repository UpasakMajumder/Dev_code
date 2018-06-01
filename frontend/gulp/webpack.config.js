/* Configuration */
const config = require('./config');
const DEVELOPMENT = process.env.isDevelopment !== 'false';

/* Modules */
const path = require('path');
const webpack = require('webpack');
const WriteFilePlugin = require('write-file-webpack-plugin');
const eslintConfig = require('eslint-config-actum').getConfig({ environment: false });
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const HappyPack = require('happypack');

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
    }),

    // ignore moment.js Locale modules
    new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
    new HappyPack({
      id: 'js',
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
    }),
    new HappyPack({
      id: 'svg',
      loaders: [
        {
          loader: 'babel-loader'
        },
        {
          loader: 'react-svg-loader',
          query: { jsx: true }
        }
      ]
    }),
    new HappyPack({
      id: 'styles',
      loaders: [
        { loader: "style-loader" },
        { loader: "css-loader" },
      ]
    })
  ],

  /* Environment-specific plugins */
  development: [
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

    // NOTE: enable to analyze JS bundles after build
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
      maxChunks: 1
    }),
    new webpack.optimize.CommonsChunkPlugin({
      name: 'common',
      filename: '[name].min.js',
      minChunks: 0
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
        [APP_ENTRY_NAME]: [config.JS_ENTRY],
        common: ['react', 'react-dom', 'react-router', 'react-transition-group', 'react-router-redux', 'redux', 'react-tippy', 'popper.js', 'lodash/isEqual', 'react-redux-toastr', 'moment', 'core-js', 'axios', 'babel-polyfill', 'whatwg-fetch']
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
                test: /\.jsx?$/,
                exclude: /node_modules/,
                include: [path.resolve(process.cwd(), config.JS_BASE)],
                loaders: ['happypack/loader?id=js']
            },
            {
                test: /\.svg$/,
                loaders: ['happypack/loader?id=svg']
            },
            {
              test: /\.css$/,
              loaders: ['happypack/loader?id=styles']
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
        extensions: ['.js', '.jsx'],
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
