Object.defineProperty(exports, "__esModule", { value: true });
const preact_1 = require("preact");
const App = () => {
    return ((0, preact_1.h)("div", null,
        (0, preact_1.h)("label", { text: "Select something to remove from your suitcase:" }),
        (0, preact_1.h)("box", null,
            (0, preact_1.h)("toggle", { name: "boots", label: "Boots", value: true }),
            (0, preact_1.h)("toggle", { name: "helmet", label: "Helmet", value: false }),
            (0, preact_1.h)("toggle", { name: "cloak", label: "Cloak of invisibility" })),
        (0, preact_1.h)("box", null,
            (0, preact_1.h)("button", { name: "cancel", text: "Cancel", onClick: (e) => log(performance.now()) }),
            (0, preact_1.h)("button", { name: "ok", text: "OK" }),
            (0, preact_1.h)("textfield", { onInput: (e) => log(e.newData) }))));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
