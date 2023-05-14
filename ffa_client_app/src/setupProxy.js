const { createProxyMiddleware } = require("http-proxy-middleware")

module.exports = function (app) {
    app.use(
        "/api",
        createProxyMiddleware({
            target: "https://localhost:7058",
            changeOrigin: false,
            logLevel: "debug",
            secure: false
        })
    )
}