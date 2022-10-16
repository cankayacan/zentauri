Object.defineProperty(exports, "__esModule", { value: true });
exports.Button = void 0;
const preact_1 = require("preact");
const hooks_1 = require("preact/hooks");
const Button = ({ style, text, enabled, onClick }) => {
    const buttonRef = (0, hooks_1.useRef)();
    (0, hooks_1.useEffect)(() => {
        buttonRef.current.ve.SetEnabled(enabled);
    }, [enabled]);
    return ((0, preact_1.h)("button", { ref: buttonRef, name: "ok", text: text, style: Object.assign(Object.assign({}, style), { borderColor: "#FFFFFF", backgroundColor: "#4E7CFF", color: enabled ? "#FFFFFF" : "#A9A9A9", fontSize: "16px" }), onClick: () => onClick() }));
};
exports.Button = Button;
