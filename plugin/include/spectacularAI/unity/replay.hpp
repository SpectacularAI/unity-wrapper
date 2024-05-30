#pragma once

#include <spectacularAI/replay.hpp>
#include "types.hpp"
#include "output.hpp"
#include "mapping.hpp"

typedef void(*callback_t_string)(const char*);

extern "C" {

    EXPORT_API spectacularAI::Replay* sai_replay_build(
        const char* folder,
        const char* configurationYAML,
        callback_t_mapper_output onMapperOutput,
        char* errorMsg);
    EXPORT_API void sai_replay_start(spectacularAI::Replay* replayHandle);
    EXPORT_API void sai_replay_run(spectacularAI::Replay* replayHandle);
    EXPORT_API bool sai_replay_one_line(spectacularAI::Replay* replayHandle);
    EXPORT_API void sai_replay_set_playback_speed(spectacularAI::Replay* replayHandle, double speed);
    EXPORT_API void sai_replay_set_dry_run(spectacularAI::Replay* replayHandle, bool isDryRun);
    EXPORT_API void sai_replay_set_output_callback(spectacularAI::Replay* replayHandle, callback_t_vio_output onOutput);
    EXPORT_API void sai_replay_release(spectacularAI::Replay* replayHandle);
}
