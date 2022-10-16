Object.defineProperty(exports, "__esModule", { value: true });
const preact_1 = require("preact");
const hooks_1 = require("preact/hooks");
const Button_1 = require("../Components/Button");
const Spinner_1 = require("../Components/Spinner");
const backgroundStyle = {
    width: "100%",
    height: "100%",
    unityBackgroundScaleMode: "ScaleAndCrop",
    backgroundImage: "images/bg.png",
};
const App = () => {
    const [loading, setLoading] = (0, hooks_1.useState)(false);
    const lobby = require("lobby");
    const playerRenderTexture = lobby.playerRenderTexture;
    const playerAnimator = lobby.playerAnimator;
    const wave = () => playerAnimator.SetTrigger("Wave");
    (0, hooks_1.useEffect)(() => {
        setInterval(() => wave(), 10000);
        wave();
    }, []);
    const startGame = () => {
        setLoading(true);
        lobby.StartGame();
    };
    return ((0, preact_1.h)("div", { class: "flex-row justify-between", style: backgroundStyle },
        (0, preact_1.h)("div", { class: "relative w-56" }, loading && ((0, preact_1.h)("div", { class: "absolute flex-row items-center bottom-5 left-5" },
            (0, preact_1.h)("div", { style: { width: 25, marginRight: 5 } },
                (0, preact_1.h)(Spinner_1.Spinner, { radius: 25, thickness: 2.5 })),
            (0, preact_1.h)("div", { style: { color: "#FFFFFF", fontSize: "8px" } }, "Loading...")))),
        (0, preact_1.h)("image", { image: playerRenderTexture, style: {
                width: "50%",
                height: "100%",
                unityBackgroundScaleMode: "ScaleAndCrop",
            } }),
        (0, preact_1.h)("div", { class: "self-end w-56 pb-5 pr-5" },
            (0, preact_1.h)(Button_1.Button, { style: {
                    borderRadius: [16, 16, 0, 16],
                }, text: "Start", onClick: startGame, enabled: !loading }))));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
