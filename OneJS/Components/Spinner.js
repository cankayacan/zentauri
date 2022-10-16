Object.defineProperty(exports, "__esModule", { value: true });
exports.Spinner = void 0;
const preact_1 = require("preact");
const hooks_1 = require("preact/hooks");
const DURATION_IN_SECONDS = 1.5;
const Spinner = ({ radius, thickness }) => {
    const loader = {
        borderWidth: thickness,
        borderColor: "#EAF0F6",
        borderRadius: 50,
        borderTopWidth: thickness,
        borderTopColor: "#4E7CFF",
        width: radius,
        height: radius,
        rotate: 0,
        transitionDuration: [DURATION_IN_SECONDS],
        transitionTimingFunction: ["EaseOutBack"],
    };
    const loaderTurning = Object.assign(Object.assign({}, loader), { rotate: 360 });
    const [turned, setTurned] = (0, hooks_1.useState)(false);
    (0, hooks_1.useEffect)(() => setTurned(true), []);
    (0, hooks_1.useEffect)(() => {
        const timeout = setTimeout(() => {
            setTurned(!turned);
        }, DURATION_IN_SECONDS * 1000);
        return () => {
            clearTimeout(timeout);
        };
    }, [turned]);
    return (0, preact_1.h)("div", { style: turned ? loaderTurning : loader });
};
exports.Spinner = Spinner;
