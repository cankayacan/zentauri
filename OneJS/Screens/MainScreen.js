Object.defineProperty(exports, "__esModule", { value: true });
const preact_1 = require("preact");
const hooks_1 = require("preact/hooks");
const App = () => {
    const lobbyPlayer = require("lobbyPlayer");
    const renderTexture = lobbyPlayer.renderTexture;
    const animator = lobbyPlayer.animator;
    log("render app");
    (0, hooks_1.useEffect)(() => {
        setInterval(() => {
            animator.SetTrigger("Wave");
        }, 10000);
    }, []);
    return ((0, preact_1.h)("div", { style: {
            backgroundImage: "images/bg.png",
            backgroundColor: "#15202C",
            width: "100%",
            height: "100%",
        }, class: "p-10" },
        (0, preact_1.h)("label", { text: "Main" }),
        (0, preact_1.h)("image", { image: renderTexture }),
        (0, preact_1.h)("div", null,
            (0, preact_1.h)("button", { name: "ok", text: "Start", style: {
                    backgroundColor: "#4E7CFF",
                    color: "#FFFFFF",
                    width: "200px",
                    height: "40px",
                    fontSize: "16px",
                    borderRadius: 5,
                } }))));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
